using API.DynamicallyData.DataSource;
using API.DynamicallyData.Helpers;
using API.DynamicallyData.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace API.DynamicallyData.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DapperRepository _dapperRepository;

        public HomeController(ILogger<HomeController> logger, DapperRepository dapperRepository)
        {
            _logger = logger;
            _dapperRepository = dapperRepository;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var tables = await _dapperRepository.GetTablesAsync();
            return View(new ViewModel { Tables = tables.SelectMany(t => t) });
        }

        [HttpGet("get-table")]
        public async Task<IActionResult> GetAuthors([FromQuery] TablesResourceParameters tableResourceParameters)
        {
            if (string.IsNullOrEmpty(tableResourceParameters?.TableName))
            {
                return BadRequest();
            }

            var rows = await _dapperRepository.GetTableAsync(tableResourceParameters.TableName, tableResourceParameters.PageSize, tableResourceParameters.PageNumber);
            var rowCount = await _dapperRepository.GetTableCountAsync(tableResourceParameters.TableName);

            int currentPage = tableResourceParameters.PageNumber + 1;
            int pageSize = tableResourceParameters.PageSize;
            int totalCount = rowCount;

            return Ok(CollectionResource.Create(rows, currentPage, pageSize, totalCount));
        }

        [HttpGet("error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
