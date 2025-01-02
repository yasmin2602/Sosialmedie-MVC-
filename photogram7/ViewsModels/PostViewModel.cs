using Microsoft.AspNetCore.Http;
namespace photogram.Models {


    public class PostViewModel
    {
        public int Id { get; set; } // to identify the posts
        public string? Content { get; set; } // edit caption 

        public string? ImagePath { get; set; } // Show existing picture
    }

    public class UpdatePostRequestModel
    {
        public int Id { get; set; } 
        public string? Content { get; set; } 
        
    }
}




   