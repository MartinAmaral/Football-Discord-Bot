using Models.Formacion;

namespace Models
{
    public class FormacionResponse
    {
        public int Results { get; set; }
        public FormacionModel[] Response { get; set; }
    }
}
