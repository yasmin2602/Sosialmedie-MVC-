using System;
using System.ComponentModel.DataAnnotations;

namespace photogram.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The comment cannot be empty.")]
        [StringLength(200, ErrorMessage = "The comment cannot exceed 200 characters.")]
        public required string Content { get; set; }

        public required string UserName { get; set; } 
        public DateTime CreatedAt { get; set; } // Creates timestamp
        public int PostId { get; set; } 
        public virtual  Post? Post { get; set; } // Navigation property to the related post entity
    }
}
