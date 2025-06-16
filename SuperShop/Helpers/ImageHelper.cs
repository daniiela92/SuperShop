using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SuperShop.Helpers
{
    public class ImageHelper : IImageHelper
    {
        public async Task<string> UploadImageAsync(IFormFile imageFile, string folder)
        {
            string guid = Guid.NewGuid().ToString(); // gera um novo GUID para garantir que o nome do ficheiro é único

            string file = $"{guid}.jpg"; // define o nome do ficheiro como o GUID seguido da extensão .jpg  


            string path = Path.Combine(
                Directory.GetCurrentDirectory(),
                $"wwwroot\\images\\{folder}",
                file); // define o caminho onde a imagem será guardada

            using (FileStream stream = new FileStream(path, FileMode.Create)) // cria um novo ficheiro ou substitui o existente
            {
                await imageFile.CopyToAsync(stream); // copia o conteúdo do ficheiro de imagem para o caminho definido
            }
            
            return $"~/images/{folder}/{file}"; // define o caminho relativo da imagem para ser usado na view
        }
    }
}
