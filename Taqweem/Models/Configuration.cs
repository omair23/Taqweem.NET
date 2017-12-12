using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Taqweem.Models
{
    public partial class Configuration
    {
        public Configuration()
        {

        }

        public Configuration(string pParameter, string pValue)
        {
            Parameter = pParameter;
            Value = pValue;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UID { get; set; }

        public string Parameter { get; set; }

        public string Value { get; set; }
    }

}
