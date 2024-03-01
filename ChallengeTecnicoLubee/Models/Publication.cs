using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace ChallengeTecnicoLubee.Models
{
    public class Publication
    {
        public int Id { get; set; }
        public required string TypeOperation {  get; set; }
        public required string TypeProperty { get; set; }
        public required string Description { get; set; }
        public int Rooms { get; set; }  
        public double SquareMeter {  get; set; }
        public required string Antiquity { get; set; }
        public required string Location {  get; set; }

        [NotMapped]
        public List<IFormFile>? Images { get; set; } = new List<IFormFile>();
        [NotMapped]
        public List<ImagePublication>? ImagesPublication { get; set; } = new List<ImagePublication>();

    }
}
