namespace PromoWeb.Common.Extensions
{
    public static class GuidExtension
    {
        public static string Shrink(this Guid guid)
        {
            return guid.ToString().Replace("-", "").Replace(" ", "");
        }

		public static string GetUniqueFileName(string fileName, int charCount)
		{
			fileName = Path.GetFileName(fileName);
			return Path.GetFileNameWithoutExtension(fileName)
					  + "_"
					  + Guid.NewGuid().ToString().Substring(0, Math.Min(charCount, 32))
					  + Path.GetExtension(fileName);
		}
	}
}
