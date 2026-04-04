using AutoTrust.Application.Common;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoTrust.Application.Interfaces.Validators
{
    public interface INotificationValidator
    {
        public Task<ValidationResult> CanCreateAsync(CreateNotificationDto dto, CancellationToken cancellationToken);
    }
}
