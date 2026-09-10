using System.ComponentModel.DataAnnotations;

namespace AbpTask.Modules.Room.Dtos.Request
{
    public class SearchRoomsRequestDto
    {
        [Required]
        [RegularExpression(
            @"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(\.\d{1,7})?(Z|[+-]\d{2}:\d{2})$",
            ErrorMessage = "The field must be a valid ISO 8601 date string (e.g., 2026-09-05T11:47:00Z)."
        )]
        public required string StartAt { get; set; }

        [RegularExpression(
            @"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(\.\d{1,7})?(Z|[+-]\d{2}:\d{2})$",
            ErrorMessage = "The field must be a valid ISO 8601 date string (e.g., 2026-09-05T11:47:00Z)."
        )]

        public required string EndAt { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be a positive integer.")]
        public required int Capacity { get; set; } 
    }
}
