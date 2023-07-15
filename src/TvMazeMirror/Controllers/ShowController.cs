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
        public async Task<IActionResult> Add([FromBody] ShowModel model) {
            logger.LogInformation("Adding Show {ShowName}", model.Name);

            try {
                var result = await addShowCommandHandler.Execute(model);

                if (result.IsValid) {
                    logger.LogInformation("Added Show {ShowName} with id {ShowId}", model.Name, result.Value);

                    return Ok(result);
                }
                else {
                    logger.LogWarning("Failed to add Show {ShowName} because of validation errors", model.Name);

                    return BadRequest(result);
                }
            }
            catch (Exception ex) {

                logger.LogError(ex, "Failed to add show {ShowName}", model.Name);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
