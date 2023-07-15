using Microsoft.AspNetCore.Mvc;
using System.Net;
using TvMazeMirror.CommandHandlers;
using TvMazeMirror.Models;

namespace TvMazeMirror.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ShowController : ControllerBase {
        private readonly IAddShowCommandHandler addShowCommandHandler;
        private readonly ILogger<ShowController> logger;

        public ShowController(IAddShowCommandHandler addShowCommandHandler, ILogger<ShowController> logger) {
            this.addShowCommandHandler = addShowCommandHandler;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Add(ShowModel model) {
            logger.LogInformation("Adding Show {Show}", model.Name);

            try {
                var result = await addShowCommandHandler.Execute(model);

                logger.LogInformation("Added Show {Show} with id {ShowId}", model.Name, result.Value);

                return Ok(result);
            }
            catch (Exception ex) {

                logger.LogError(ex, "Failed to add show {Show}", model.Name);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
