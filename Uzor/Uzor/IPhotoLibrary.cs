using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Uzor
{
    public interface IPhotoLibrary
    {
        Task<string> SavePhotoAsync(byte[] data, string folder, string filename);
    }
}
