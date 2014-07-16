// ----------------------------------------------------------------
// Skyrim Manager
// Copyright (c) 2013. Zack Loveless.
// 
// Original author(s) for this source file: Zack Loveless
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
// ----------------------------------------------------------------

namespace Skyrim.Manager.Models
{
	using System;
	using System.Collections.ObjectModel;
	using System.IO;
	using System.Text;
	using System.Threading.Tasks;
	using Newtonsoft.Json;
	using ViewModels;

    [JsonObject]
    public class Settings : Model
    {
        private string path;

        #region Properties

        [JsonProperty]
        public CharacterSettings CharactersConfig { get; set; }

        [JsonProperty]
        public LauncherSettings Launch { get; set; }

        #endregion

        #region Factory methods

        public static async Task<Settings> Load(string file)
        {
            using (var s = new FileStream(file, FileMode.OpenOrCreate))
            using (var reader = new StreamReader(s, Encoding.UTF8))
            {
                string json = await reader.ReadToEndAsync();

                var result = JsonConvert.DeserializeObject<Settings>(json);
                result.path = file;

                return result;
            }
        }

        public static async Task Save(Settings settings, string file = null)
        {
            string json = JsonConvert.SerializeObject(settings, Formatting.Indented);

            if (String.IsNullOrEmpty(file))
            {
                file = settings.path;
            }

            using (var s = new FileStream(file, FileMode.Truncate))
            using (var writer = new StreamWriter(s, new UTF8Encoding(false)))
            {
                await writer.WriteAsync(json);
            }
        }

        #endregion

        #region Nested type: LauncherSettings

        [JsonObject]
        public class LauncherSettings : Model
        {
            private string executable;
            private string installPath;
            private string dataPath;

            #region Properties

            [JsonProperty]
            public string Executable
            {
                get { return executable; }
                set
                {
                    OnPropertyChanging();
                    executable = value;
                    OnPropertyChanged();
                }
            }

            [JsonProperty]
            public string InstallPath
            {
                get { return installPath; }
                set
                {
                    OnPropertyChanging();
                    installPath = value;
                    OnPropertyChanged();
                }
            }

            [JsonProperty]
            public string DataPath
            {
                get { return dataPath; }
                set
                {
                    OnPropertyChanging();
                    dataPath = value;
                    OnPropertyChanged();
                }
            }

            #endregion
        }

        #endregion

        #region Nested type: CharacterSettings

        [JsonObject]
        public class CharacterSettings : Model
        {
            [JsonProperty]
            public string LastSelected { get; set; }

            [JsonProperty("list")]
            public ObservableCollection<string> Characters { get; set; }
        }

        #endregion
    }
}
