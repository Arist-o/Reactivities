using Application.Activities.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using Application.Activities.DTOs;
namespace Application.Activities.Validators
{
    public class EditActivityValidator : BaseActivityValidator<EditActivity.Command, EditActivityDto>
    {
        public EditActivityValidator() : base(x => x.ActivityDto)
        {

        }
    }
}
