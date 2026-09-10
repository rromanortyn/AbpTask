using AbpTask.Data;
using AbpTask.Data.Entities;
using AbpTask.Modules.Reservation.UseCases.Inputs;
using AbpTask.Modules.Reservation.UseCases.Interfaces;
using AbpTask.Modules.Reservation.UseCases.Outputs;
using AbpTask.Modules.Room.UseCases.Classes;
using Microsoft.EntityFrameworkCore;

namespace AbpTask.Modules.Reservation.UseCases
{
    public class CreateReservationUseCase : ICreateReservationUseCase
    {
        private readonly ApplicationDbContext _context;
        private readonly List<HourRange> _hourRanges = [
            new () {
                    Start = TimeSpan.FromHours(6),
                End = TimeSpan.FromHours(9),
                Type = HourRange.HourRangeType.Morning
            },
            new () {
                Start = TimeSpan.FromHours(9),
                End = TimeSpan.FromHours(12),
                Type = HourRange.HourRangeType.Standard
            },
            new () {
                Start = TimeSpan.FromHours(12),
                End = TimeSpan.FromHours(14),
                Type = HourRange.HourRangeType.Rush
            },
            new () {
                Start = TimeSpan.FromHours(14),
                End = TimeSpan.FromHours(18),
                Type = HourRange.HourRangeType.Standard
            },
            new () {
                Start = TimeSpan.FromHours(18),
                End = TimeSpan.FromHours(23),
                Type = HourRange.HourRangeType.Evening
            },
        ];

        private readonly Dictionary<HourRange.HourRangeType, double> _priceCoefficients = new()
        {
            { HourRange.HourRangeType.Morning, 0.9 },
            { HourRange.HourRangeType.Standard, 1 },
            { HourRange.HourRangeType.Rush, 1.15 },
            { HourRange.HourRangeType.Evening, 0.8 },
        };

        public CreateReservationUseCase(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CreateReservationUseCaseOutput> Execute(CreateReservationUseCaseInput input)
        {
            var roomById = await _context.Rooms.FindAsync(input.RoomId);

            if (roomById == null)
            {
                throw new Exception("Room not found");
            }

            var hasConflictByTime = await _context.Reservations
                .AnyAsync(reservation => reservation.RoomId == input.RoomId && reservation.StartAt <= input.EndAt.ToUniversalTime() && reservation.EndAt >= input.StartAt.ToUniversalTime());

            if (hasConflictByTime)
            {
                throw new Exception("conflict by time");
            }

            var services = await _context.Services
                .Where(s => input.ChosenServicesIds.Contains(s.Id) && s.RoomId == input.RoomId)
                .ToListAsync();

            if (services.Count < input.ChosenServicesIds.Count)
            {
                throw new Exception("One or more services not found");
            }

            TimeSpan start = input.StartAt.TimeOfDay;
            TimeSpan end = input.EndAt.TimeOfDay;

            var reservationDuration = end - start;
            var bestRange = _hourRanges
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
                    Percentage = x.Overlap.TotalMinutes / (x.Range.End - x.Range.Start).TotalMinutes * 100,
                    Coefficient = _priceCoefficients[x.Range.Type]
                })
                .OrderByDescending(x => x.Coefficient)
                .ThenByDescending(x => x.Percentage)
                .FirstOrDefault();

            var priceForServices = services.Sum(service => service.Price);
            var priceForRoom = roomById.BasePricePerHour * reservationDuration.TotalHours * bestRange.Coefficient;
            var totalPrice = priceForServices + priceForRoom;

            var reservation = new ReservationEntity
            {
                RoomId = input.RoomId,
                StartAt = input.StartAt.ToUniversalTime(),
                EndAt = input.EndAt.ToUniversalTime(),
                ChosenServices = [.. services.Select(service => new ChosenServiceEntity
                {
                    ServiceId = service.Id,
                    Price = service.Price,
                })]
            };

            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();

            return new CreateReservationUseCaseOutput
            {
                Id = reservation.Id,
                StartAt = reservation.StartAt,
                EndAt = reservation.EndAt,
                TotalPrice = totalPrice,
                Services = [.. services.Select(s => new CreateReservationUseCaseOutput.Service
                {
                    Id = s.Id,
                    Name = s.Name,
                    Price = s.Price
                })],
                RoomDetails = new CreateReservationUseCaseOutput.Room
                {
                    Id = roomById.Id,
                    Name = roomById.Name,
                    Capacity = roomById.Capacity,
                    BasePricePerHour = roomById.BasePricePerHour,
                    PricePercentage = (int)Math.Round(bestRange.Coefficient * 100),
                }
            };
        }
    }
}
