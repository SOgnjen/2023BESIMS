using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalLibrary.Core.Model
{
    public class Blog
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int WriterJmbg { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Text { get; set; }

        public Blog(int writerJmbg, string title, string text)
        {
            WriterJmbg = writerJmbg;
            Title = title;
            Text = text;
        }

        public Blog() {}
    }
}
