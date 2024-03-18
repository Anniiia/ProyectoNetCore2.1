namespace ProyectoNetCore.Models
{
    public class SerieBarra
    {
        public SerieBarra() { }

        public object[] GetDataDum()
        {
            object[] data = new object[5];
            data[0] = new object[] { "Angular",10 };
            data[1] = new object[] { "CSS",50 };
            data[2] = new object[] { "JS", 25 };
            data[3] = new object[] { "Vue", 30 };
            data[4] = new object[] { "React",40 };

            return data;
        }
    }
}
