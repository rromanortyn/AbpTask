using System.ComponentModel.DataAnnotations;

namespace AbpTask.Modules.Reservation.Dtos.Request
{
    public class CreateReservationRequestDto
    {
        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "RoomId must be a positive number.")]
        public long RoomId { get; set; }
        [Required]
        [RegularExpression(
            @"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(\.\d{1,7})?(Z|[+-]\d{2}:\d{2})$",
            ErrorMessage = "The field must be a valid ISO 8601 date string (e.g., 2026-09-05T11:47:00Z)."
        )]
        public string StartAt { get; set; } = string.Empty;

        [Required]
        [RegularExpression(
            @"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(\.\d{1,7})?(Z|[+-]\d{2}:\d{2})$",
            ErrorMessage = "The field must be a valid ISO 8601 date string (e.g., 2026-09-05T11:47:00Z)."
        )]
        public string EndAt { get; set; } = string.Empty;

        [Required]
        public List<long> ChosenServicesIds { get; set; } = [];

    }
}
