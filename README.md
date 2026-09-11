# Implementation

The application is implemented as a REST API using **ASP.NET Core** with **PostgreSQL** as the relational database.

The main technologies used are:

- **ASP.NET Core** — API endpoints and application configuration
- **Entity Framework Core** — database access, entity relationships and migrations
- **PostgreSQL** — persistent data storage
- **AutoMapper** — mapping between entities and request/response DTOs
- **JsonPatch** — partial updates of existing resources through `PATCH` endpoints

The application separates API models from persistence entities and uses DTOs for communication with clients. Entity Framework Core is responsible for querying and persisting domain data, while AutoMapper reduces repetitive mapping code between the API and database layers.

## Reservation price calculation

Reservation pricing depends on the room's base hourly price and the pricing coefficient associated with different periods of the day.

Each configured hour range contains:

- start time;
- end time;
- pricing type;
- price coefficient.

### Calculating intersections

A reservation can intersect several pricing ranges. For every range, the application calculates the duration of the intersection using:

```csharp
overlap =
    Math.Max(
        0,
        Math.Min(reservationEnd, rangeEnd)
        - Math.Max(reservationStart, rangeStart)
    );
```

Conceptually, the intersection:

1. starts at the later of the reservation start and range start;
2. ends at the earlier of the reservation end and range end;
3. is treated as zero when the resulting duration is negative.

In other words:

```text
overlap start = latest start
overlap end   = earliest end
```

Ranges that do not intersect the reservation are excluded from further calculations.

### Range coverage

For every intersecting range, the application calculates how much of that **pricing range** is covered by the reservation:

```text
coverage = overlap duration / pricing range duration
```

For example, for a rush-hour range from `12:00` to `14:00`, a reservation covering the whole period has:

```text
2 hours overlap / 2 hours range duration = 100%
```

If a reservation covers three hours of a four-hour standard period:

```text
3 / 4 = 75%
```

Using the pricing range itself as the denominator makes it possible to compare how fully each configured range is covered by the reservation.

### Selecting the applicable coefficient

After calculating all intersections, only ranges with a positive overlap are considered.

The selection process is:

```text
calculate intersections
        ↓
remove ranges with no intersection
        ↓
calculate percentage of each pricing range covered
        ↓
sort by price coefficient descending
        ↓
sort equal coefficients by coverage descending
        ↓
select the first range
```

The price coefficient is therefore the primary criterion.

This is important for rush-hour pricing. Consider a reservation that contains:

```text
Standard period: 3 hours
Rush period:     2 hours
```

Even though the reservation spends more absolute time in the standard period, the rush-hour range is selected because it has a higher pricing coefficient.

This avoids relying only on the longest intersection, which could incorrectly select standard pricing for reservations that overlap a higher-demand period.

A simplified LINQ representation of the algorithm is:

```csharp
var bestRange = hourRanges
    .Select(range => new
    {
        Range = range,
        Overlap = TimeSpan.FromTicks(
            Math.Max(
                0,
                Math.Min(end.Ticks, range.End.Ticks)
                - Math.Max(start.Ticks, range.Start.Ticks)
            )
        )
    })
    .Where(x => x.Overlap > TimeSpan.Zero)
    .Select(x => new
    {
        x.Range,
        x.Overlap,
        Percentage =
            x.Overlap.TotalMinutes /
            (x.Range.End - x.Range.Start).TotalMinutes,
        Coefficient = priceCoefficients[x.Range.Type]
    })
    .OrderByDescending(x => x.Coefficient)
    .ThenByDescending(x => x.Percentage)
    .FirstOrDefault();
```

The selected coefficient is then applied to the room's base hourly price when calculating the reservation price.

Additional services are calculated separately and added to the adjusted room rental price.

## Partial updates

Resources that support partial modification use **JSON Patch** rather than requiring the client to send the complete object.

ASP.NET Core's `JsonPatchDocument` is used to apply the requested operations to an update DTO. The resulting model is validated and then mapped back to the corresponding entity.

This allows clients to update only individual fields while keeping the API's update behavior explicit and consistent.

## Data persistence

**Entity Framework Core** is used as the ORM and **PostgreSQL** as the database engine.

EF Core provides:

- entity mapping;
- relationships between database models;
- asynchronous queries;
- change tracking;
- persistence;
- database migrations.

Database entities remain separated from public API DTOs, with **AutoMapper** handling conversion between these representations.