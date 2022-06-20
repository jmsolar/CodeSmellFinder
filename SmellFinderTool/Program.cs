using SmellFinderTool.Utils;

namespace SmellFinderTool
{
    public class Program
    {
        static void Main(string[] args)
        {
            var path = CodeSmellSelector.SelectDirectory();
            var smells = CodeSmellSelector.SelectSmell();

            ReaderHelper.ProcessDirectoryPath(path, smells);

            // Proceso directorio
            // Creo directorio de salida
            // Genero un archivo json para informe haciendo append de la informacion obtenida
        }
    }
}
