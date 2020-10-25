using Assets.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace Assets.Controllers
{
    public static class ErrorHelper
    {
        public static void ShowIODataError(Exception exception)
        {
            string error = string.Format("{0}\n{1}",exception.Message, exception.InnerException?.Message);
            MainManager.CanvasManager.ShowMessage("Wystąpił błąd z zapisem / odczytem pliku: " + "\n" +  error);
        }
    }
}
