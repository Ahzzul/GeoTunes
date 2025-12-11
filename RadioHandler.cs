using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace GeoTunes
{
    class RadioHandler
    {
        Random rnd = new Random();
        AudioPlayer player = new AudioPlayer();
        
        public async Task PlayRadio(CityDistance[] cities, string radioPath)
        {
            radioPath = GetClosestRadioStation(cities, radioPath);
            if (!Directory.Exists(radioPath))
            {
                throw new DirectoryNotFoundException("Folder does not Exist: " +  radioPath);
            }
            string[] files = Directory.GetFiles(radioPath);
            if (files.Length == 0)
            {
                return;
            }
            int index = rnd.Next(files.Length);
            if (files.Length > 1)
            {
                await player.PlayMp3Async(files, index, true);
            }
            else
            {
                await player.PlayMp3Async(files, index, false);
            }
        }

        private string GetClosestRadioStation(CityDistance[] cities, string radioPath)
        {
            string newRadioPath;
            foreach (CityDistance cd in cities)
            {
                newRadioPath = radioPath + @"\" + cd.CityName;
                if (Directory.Exists(radioPath + @"\" + cd.CityName))
                {
                    return newRadioPath;
                }
            }
            return "Null";
        }
    }
}
