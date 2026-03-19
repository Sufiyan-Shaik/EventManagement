using System.ComponentModel.DataAnnotations;

namespace EventManagement.Models
{
    // represents a single event in the system
    public class Event
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date is required")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Time is required")]
        [DataType(DataType.Time)]
        public TimeSpan Time { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [StringLength(200)]
        public string Location { get; set; } = string.Empty;

        // who created this event
        public string? UserId { get; set; }
    }
}