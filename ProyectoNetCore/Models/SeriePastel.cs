namespace ProyectoNetCore.Models
{
    public class SeriePastel
    {
        //name: 'Chrome',
        //        y: 74.77,
        //        sliced: true,
        //        selected: true

        public string name { get; set; }
        public double y { get; set; }
        public bool sliced{ get; set; }
        public bool selected { get; set; }

        //public SeriePastel(string name, double y, bool sliced = false, bool selected = false)
        //{
        //    this.name = name;
        //    this.y = y;
        //    this.sliced = sliced;
        //    this.selected = selected;
        //}

        //public List<SeriePastel> GetDataDum()
        //{
        //    List<SeriePastel> lista = new List<SeriePastel>();
        //    lista.Add(new SeriePastel("Angular", 45));
        //    lista.Add(new SeriePastel("Vue", 50));
        //    lista.Add(new SeriePastel("React", 30));
        //    lista.Add(new SeriePastel("Css", 20));

        //    return lista;
        //}
    }
}
