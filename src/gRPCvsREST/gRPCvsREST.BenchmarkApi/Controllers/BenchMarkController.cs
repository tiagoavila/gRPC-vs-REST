using gRPCvsREST.BenchmarkApi.Services;
using gRPCvsREST.Core.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace gRPCvsREST.BenchmarkApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BenchMarkController : ControllerBase
    {
        private readonly ServerApiService _serverApiService;

        public BenchMarkController(ServerApiService serverApiService)
        {
            _serverApiService = serverApiService;
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetAsync()
        {
            List<OrderDto> orders = await _serverApiService.GetOrdersUsingRestAsync();
            return Ok(orders);
        }

        [HttpGet("get-grpc")]
        public async Task<IActionResult> GetGrpcAsync()
        {
            List<OrderDto> orders = await _serverApiService.GetOrdersUsingGrpcAsync();
            return Ok(orders);
        }

        [HttpGet("get-time-of-requests")]
        public async Task<IActionResult> GetTimeOfRequestsAsync()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            List<OrderDto> orders = await _serverApiService.GetOrdersUsingRestAsync();

            stopwatch.Stop();
            TimeSpan timeTakenRest = stopwatch.Elapsed;

            stopwatch.Reset();
            stopwatch.Start();
            List<OrderDto> ordersGrpc = await _serverApiService.GetOrdersUsingGrpcAsync();
            stopwatch.Stop();
            TimeSpan timeTakenGrpc = stopwatch.Elapsed;

            return Ok(new
            {
                TimeTakenRest = timeTakenRest.ToString(@"m\:ss\.fff"),
                TimeTakenGrpc = timeTakenGrpc.ToString(@"m\:ss\.fff")
            });
        }
    }
}
