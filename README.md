# Reservation API

A REST API for managing rooms and reservations, implemented with **ASP.NET Core** and **PostgreSQL**.

The application supports room management, reservation creation, dynamic reservation pricing, partial resource updates, and OpenAPI documentation through Scalar.

## Tech Stack

* **ASP.NET Core** — REST API and controller routing
* **Entity Framework Core** — ORM and database access
* **PostgreSQL** — relational database
* **AutoMapper** — mapping between entities and DTOs
* **JsonPatch** — partial updates through `PATCH` requests
* **DataAnnotations** — request validation
* **Scalar** — OpenAPI documentation UI

## API Routes

### Rooms

#### Create room

```http
POST /api/rooms
```

Creates a new room.

#### Get rooms

```http
GET /api/rooms
```

Returns the available rooms.

#### Update room

```http
PATCH /api/rooms/{id}
```

Partially updates an existing room using **JSON Patch**.

It's important to set the correct content type:

```http
Content-Type: application/json-patch+json
```

#### Delete room

```http
DELETE /api/rooms/{id}
```

Deletes a room by its identifier.

### Reservations

#### Create reservation

```http
POST /api/reservations
```

Creates a new reservation and calculates its price according to the room rate, reservation duration, selected services, and applicable time-based pricing coefficient.

## Reservation Price Calculation

Rooms have a base hourly price. Different periods of the day may have different price coefficients, such as morning, standard, rush-hour, or evening pricing.

When creating a reservation, the application calculates how much the reservation overlaps each configured pricing range.

The overlap is calculated as:

```text
overlap start = latest start time
overlap end   = earliest end time
overlap       = max(0, overlap end - overlap start)
```

In code, the calculation is equivalent to:

```csharp
Math.Max(
    0,
    Math.Min(reservationEnd.Ticks, rangeEnd.Ticks)
    - Math.Max(reservationStart.Ticks, rangeStart.Ticks)
)
```

Ranges with no intersection are ignored.

For each intersecting range, the percentage of that pricing range covered by the reservation is also calculated:

```text
coverage = overlap duration / pricing range duration
```

The applicable range is selected primarily by the **highest price coefficient**. Coverage percentage is used as a secondary sorting criterion when coefficients are equal.

This ensures that, for example, a reservation containing both standard and rush-hour periods can use the rush-hour coefficient even if the absolute standard-period overlap is longer.

The selected coefficient is applied to the **room rental price**, while additional services are added separately.

Conceptually:

```text
room price =
    base hourly price
    × reservation duration
    × pricing coefficient

total reservation price =
    room price
    + additional services
```

## Validation

Request models use **DataAnnotations** for validation.

Invalid requests are automatically returned as `400 Bad Request` responses by ASP.NET Core.

## Partial Updates

Room updates are implemented using **JsonPatch**.

This allows clients to modify individual fields without sending the complete resource.

Example:

```json
[
  {
    "op": "replace",
    "path": "/name",
    "value": "Conference Room"
  }
]
```

## Data Access

**Entity Framework Core** is used to communicate with PostgreSQL.

The application uses separate entity and DTO models. **AutoMapper** handles transformations between persistence models and API request/response models.

## API Documentation

Interactive OpenAPI documentation is available through **Scalar** at:

```text
/scalar/v1
```

The documentation contains available routes, request and response schemas, validation responses, and supported HTTP status codes.
