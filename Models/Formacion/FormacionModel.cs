namespace Models.Formacion
{
    public class FormacionModel
    {
        public TeamModel Team { get; set; }
        public string formation { get; set; }
        public PlayerModel[] startXI { get; set; }
    }
}
