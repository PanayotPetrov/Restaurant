﻿namespace Restaurant.Web.ViewModels.InputModels
{
    using System.ComponentModel.DataAnnotations;

    public class AddReviewInputModel
    {
        public string ApplicationUserId { get; set; }

        [Required]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please select a rating!")]
        public string Rating { get; set; }
    }
}