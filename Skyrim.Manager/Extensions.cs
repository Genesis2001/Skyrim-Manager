// -----------------------------------------------------------------------------
//  <copyright file="Extensions.cs" company="Zack Loveless">
//      Copyright (c) Zack Loveless.  All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------------
namespace Skyrim.Manager
{
	using System;
	using System.IO;
	using System.Linq;
	using System.Reflection;

	public static class Extensions
	{
		public static bool SaveResource(this Assembly source, string resourceName, string targetPath)
		{
			if (source == null) throw new ArgumentNullException("source");

			string[] resources = source.GetManifestResourceNames();
			string resource = resources.SingleOrDefault(x => x.EndsWith(resourceName, StringComparison.OrdinalIgnoreCase));

			using (var s = (UnmanagedMemoryStream)source.GetManifestResourceStream(resource))
			{
				if (s == null)
				{
					throw new InvalidResourceException(resource);
				}

				var buf = new byte[s.Length];
				s.Read(buf, 0, buf.Length);

				using (var writer = new BinaryWriter(File.Open(targetPath, FileMode.Create)))
				{
					writer.Write(buf);
				}
			}

			return File.Exists(targetPath) && (new FileInfo(targetPath).Length > 0);
		}
	}
}
