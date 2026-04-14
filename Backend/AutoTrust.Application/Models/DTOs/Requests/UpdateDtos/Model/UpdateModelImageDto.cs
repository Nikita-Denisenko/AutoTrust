using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Model
{
    public record UpdateModelImageDto
    (
        [Url]
        string ImageUrl
    );
}
