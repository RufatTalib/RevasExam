namespace Revas.Helpers
{
    public static class FileManager
    {
        public static void Save(this IFormFile file, string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
            }
        }

        public static string Save(this IFormFile file, params string[] paths)
        {
            string filename = file.FileName;

            Guid guid = Guid.NewGuid();

            if (filename.Length > 64)
                filename = guid + "_" + filename.Substring(filename.Length - 64);
            else
                filename = guid + "_" + filename;

            string savePath = Path.Combine(Path.Combine(paths), filename);

            file.Save(savePath);

            return filename;
        }
    }
}
