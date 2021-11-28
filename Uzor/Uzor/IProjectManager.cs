using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Uzor
{
    public interface IProjectManager
    {
        Task<string[]> ExportProjects();
        Task<string[]> ImportProjects();
        string GetExternalFolderPath();
    }
}
