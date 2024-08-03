using COSUMIR_API_TEST.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using COSUMIR_API_TEST.Servicios;

namespace COSUMIR_API_TEST.Controllers
{
    public class HomeController : Controller
    {
        private IServicio_API _servicioApi;

        public HomeController(IServicio_API servicioApi)
        {
            _servicioApi = servicioApi;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                List<Producto> lista = await _servicioApi.Lista();
                return View(lista);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Index: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, StackTrace = ex.StackTrace });
            }
        }

        public async Task<IActionResult> Producto(int idProducto)
        {
            try
            {
                Producto modelo_producto = new Producto();
                ViewBag.Accion = "Nuevo Producto";

                if (idProducto != 0)
                {
                    ViewBag.Accion = "Editar Producto";
                    modelo_producto = await _servicioApi.Obtener(idProducto);
                }

                return View(modelo_producto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Producto: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, StackTrace = ex.StackTrace });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GuardarCambios(Producto ob_producto)
        {
            try
            {
                bool respuesta;

                if (ob_producto.IdProducto == 0)
                {
                    respuesta = await _servicioApi.Guardar(ob_producto);
                }
                else
                {
                    respuesta = await _servicioApi.Editar(ob_producto);
                }

                if (respuesta)
                    return RedirectToAction("Index");
                else
                    throw new Exception("Error al guardar o editar el producto.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GuardarCambios: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, StackTrace = ex.StackTrace });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int idProducto)
        {
            try
            {
                var respuesta = await _servicioApi.Eliminar(idProducto);

                if (respuesta)
                    return RedirectToAction("Index");
                else
                    throw new Exception("Error al eliminar el producto.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Eliminar: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, StackTrace = ex.StackTrace });
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}