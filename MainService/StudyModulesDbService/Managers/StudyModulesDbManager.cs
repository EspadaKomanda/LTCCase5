using StudyModulesDbService.Database;
using StudyModulesDbService.Database.Models;
using Grpc.Core;
using System.ComponentModel.DataAnnotations;
using StudyModulesDbService.Services;
using System.Net.Mail;

namespace StudyModulesDbService.Managers
{
    public class StudyModulesDbManager
    {

        public async Task<string> CreateStudyModule(StudyModuleModel request)
        {
            string ValidationResult = Validate(request);
            if (ValidationResult == "")
            {
                try
                {
                    using (ApplicationContext ctx = new ApplicationContext())
                    {
                        var state = await ctx.studyModules.AddAsync(new StudyModuleModel()
                        {
                            uuid = Guid.NewGuid(),
                            name = request.name,
                            asignee = request.asignee
                        });
                        await ctx.SaveChangesAsync();
                        if (state.State == Microsoft.EntityFrameworkCore.EntityState.Added)
                        {
                            return "Added big module successfully";
                        }
                        return "EntityState.Added is false";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return ex.ToString();
                }
            }
            else
            {
                return ValidationResult;
            }
        }

        public async Task<string> ModifyStudyModule(StudyModuleModel request)
        {
            string ValidationResult = Validate(request);
            if (ValidationResult == "")
            {
                try
                {
                    using (ApplicationContext ctx = new ApplicationContext())
                    {
                        var studyModule = ctx.studyModules.Where(p => p.uuid == request.uuid).ToList()[0];
                        if (studyModule != null)
                        {
                            studyModule.name = request.name ?? studyModule.name;
                            studyModule.asignee = request.asignee ?? studyModule.asignee;

                            await ctx.SaveChangesAsync();
                            return "";
                        }

                        return $"StudyModule ({request.uuid} does not exist";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return ex.ToString();
                }
            }
            else
            {
                return ValidationResult;
            }
        }

        public async Task<string> DeleteStudyModule(StudyModuleModel request)
        {
            try
            {
                using (ApplicationContext ctx = new ApplicationContext())
                {
                    var studyModule = await ctx.studyModules.FindAsync(request.uuid);
                    if (studyModule != null)
                    {
                        ctx.studyModules.Remove(studyModule);
                        await ctx.SaveChangesAsync();
                        return "";
                    }

                    return String.Format("User ({0} does not exist", request.uuid);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return ex.ToString();
            }
        }

        public async Task<StudyModuleModel> GetStudyModule(StudyModuleModel request)
        {
            try
            {
                var studyModule = new StudyModuleModel();
                using (ApplicationContext ctx = new ApplicationContext())
                {

                    studyModule = await ctx.studyModules.FindAsync(request.uuid);
                }

                if (studyModule == null)
                {
                    return await Task.FromResult(new StudyModuleModel());
                }

                return await Task.FromResult(studyModule);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new StudyModuleModel();
            }
        }

        public async Task<StudyModule> GetStudyModuleWhole(StudyModuleModel request)
        {
            try
            {
                var studyModule = new StudyModuleModel();
                using (ApplicationContext ctx = new ApplicationContext())
                {
                    studyModule = await ctx.studyModules.FindAsync(request.uuid);

                    if (studyModule == null)
                    {
                        return await Task.FromResult(new StudyModule());
                    }

                    var getWholeSubmodule = (StudySubmoduleModel submoduleModel) => {
                        var uuid = submoduleModel.uuid;

                        if (submoduleModel == null)
                        {
                            return new StudySubmodule();
                        }

                        var attachmentModels = ctx.attachments.Where(x => x.parentId == uuid);
                        var attachments = 
                            (from att
                            in attachmentModels
                            select new Services.Attachment()
                            {
                                ParentId = att.parentId.ToString(),
                                Url = att.url,
                                Uuid = att.uuid.ToString()
                            }).ToList();
                        var resultSubmodule = new StudySubmodule()
                        {
                            Name = submoduleModel.name,
                            ParentId = submoduleModel.parentId.ToString(),
                            Text = submoduleModel.text,
                            Uuid = submoduleModel.uuid.ToString(),
                        };
                        resultSubmodule.Attachments.AddRange(attachments);
                        return resultSubmodule;
                    };

                    var studySubmodules =
                        (from i in ctx.studySubmodules
                         where i.parentId == request.uuid
                         select getWholeSubmodule(i)).ToList();

                    var tests = from test in ctx.tests
                                where (test.parentId == request.uuid)
                                select new Test()
                                {
                                    ParentId= test.parentId.ToString(),
                                    Url = test.url,
                                    Uuid= test.uuid.ToString()
                                };

                    var deadlines = from deadline in ctx.deadlines
                                where (deadline.parentId == request.uuid)
                                select new Deadline()
                                {
                                    ParentId = deadline.parentId.ToString(),
                                    UserId = deadline.userId.ToString(),
                                    Datetime = deadline.deadline.ToString(),
                                    Uuid = deadline.uuid.ToString()
                                };

                    var studyModuleWhole = new StudyModule() {
                        Name = studyModule.name,
                        Asignee = studyModule.asignee,
                        Uuid = studyModule.uuid.ToString()
                    };
                    studyModuleWhole.Submodules.AddRange(studySubmodules);
                    studyModuleWhole.Tests.AddRange(tests);
                    studyModuleWhole.Deadlines.AddRange(deadlines);

                    return await Task.FromResult(studyModuleWhole);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new StudyModule();
            }
        }

        public async Task<StudyModulesAll> GetStudyModulesAll(GetStudyModulesAllRequest request)
        {
            try
            {
                using (ApplicationContext ctx = new ApplicationContext())
                {
                    var modules = (from module in ctx.studyModules
                                  select GetStudyModuleWhole(module).Result).ToList();

                    var modulesMsg = new StudyModulesAll();
                    modulesMsg.StudyModules.AddRange(modules);
                    
                    return await Task.FromResult(modulesMsg);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return await Task.FromResult(new StudyModulesAll());
            }
        }

        public async Task<string> CreateStudySubmodule(StudySubmoduleModel request)
        {
            string ValidationResult = Validate(request);
            if (ValidationResult == "")
            {
                try
                {
                    using (ApplicationContext ctx = new ApplicationContext())
                    {
                        var state = await ctx.studySubmodules.AddAsync(new StudySubmoduleModel()
                        {
                            uuid = Guid.NewGuid(),
                            name = request.name,
                            parentId = request.parentId,
                            text = request.text,
                        });
                        await ctx.SaveChangesAsync();
                        if (state.State == Microsoft.EntityFrameworkCore.EntityState.Added)
                        {
                            return "Added small module successfully";
                        }
                        return "EntityState.Added is false";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return ex.ToString();
                }
            }
            else
            {
                return ValidationResult;
            }
        }

        public async Task<string> ModifyStudySubmodule(StudySubmoduleModel request)
        {
            string ValidationResult = Validate(request);
            if (ValidationResult == "")
            {
                try
                {
                    using (ApplicationContext ctx = new ApplicationContext())
                    {
                        var studySubmodule = ctx.studySubmodules.Where(p => p.uuid == request.uuid).ToList()[0];

                        if (studySubmodule != null)
                        {
                            studySubmodule.name = request.name ?? studySubmodule.name;
                            studySubmodule.parentId = (request.parentId.ToString() == "") ? request.parentId : studySubmodule.parentId;
                            studySubmodule.text = request.text ?? studySubmodule.text;
                            await ctx.SaveChangesAsync();
                            return "";
                        }

                        return $"StudySubmodule {request.uuid} does not exist";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return ex.ToString();
                }
            }
            else
            {
                return ValidationResult;
            }
        }

        public async Task<string> DeleteStudySubmodule(StudySubmoduleModel request)
        {
            try
            {
                using (ApplicationContext ctx = new ApplicationContext())
                {
                    var studySubmodule = await ctx.studySubmodules.FindAsync(request.uuid);
                    if (studySubmodule != null)
                    {
                        ctx.studySubmodules.Remove(studySubmodule);
                        await ctx.SaveChangesAsync();
                        return "";
                    }

                    return String.Format("User ({0} does not exist", request.uuid);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return ex.ToString();
            }
        }

        public async Task<StudySubmoduleModel> GetStudySubmodule(StudySubmoduleModel request)
        {
            try
            {
                var studySubmodule = new StudySubmoduleModel();
                using (ApplicationContext ctx = new ApplicationContext())
                {

                    studySubmodule = await ctx.studySubmodules.FindAsync(request.uuid);
                }

                if (studySubmodule == null)
                {
                    return await Task.FromResult(new StudySubmoduleModel());
                }

                return await Task.FromResult(studySubmodule);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new StudySubmoduleModel();
            }
        }

        public async Task<string> CreateAttachment(AttachmentModel request)
        {
            string ValidationResult = Validate(request);
            if (ValidationResult == "")
            {
                try
                {
                    using (ApplicationContext ctx = new ApplicationContext())
                    {
                        var state = await ctx.attachments.AddAsync(new AttachmentModel()
                        {
                            uuid = Guid.NewGuid(),
                            parentId = request.parentId,
                            url = request.url,
                        });
                        await ctx.SaveChangesAsync();
                        if (state.State == Microsoft.EntityFrameworkCore.EntityState.Added)
                        {
                            return "Added small module successfully";
                        }
                        return "EntityState.Added is false";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return ex.ToString();
                }
            }
            else
            {
                return ValidationResult;
            }
        }

        public async Task<string> ModifyAttachment(AttachmentModel request)
        {
            string ValidationResult = Validate(request);
            if (ValidationResult == "")
            {
                try
                {
                    using (ApplicationContext ctx = new ApplicationContext())
                    {
                        var Attachment = ctx.attachments.Where(p => p.uuid == request.uuid).ToList()[0];

                        if (Attachment != null)
                        {
                            Attachment.parentId = (request.parentId.ToString() == "") ? request.parentId : Attachment.parentId;
                            Attachment.url = request.url ?? Attachment.url;
                            await ctx.SaveChangesAsync();
                            return "";
                        }

                        return $"Attachment {request.uuid} does not exist";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return ex.ToString();
                }
            }
            else
            {
                return ValidationResult;
            }
        }

        public async Task<string> DeleteAttachment(AttachmentModel request)
        {
            try
            {
                using (ApplicationContext ctx = new ApplicationContext())
                {
                    var Attachment = await ctx.attachments.FindAsync(request.uuid);
                    if (Attachment != null)
                    {
                        ctx.attachments.Remove(Attachment);
                        await ctx.SaveChangesAsync();
                        return "";
                    }

                    return String.Format("User ({0} does not exist", request.uuid);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return ex.ToString();
            }
        }

        public async Task<AttachmentModel> GetAttachment(AttachmentModel request)
        {
            try
            {
                var Attachment = new AttachmentModel();
                using (ApplicationContext ctx = new ApplicationContext())
                {

                    Attachment = await ctx.attachments.FindAsync(request.uuid);
                }

                if (Attachment == null)
                {
                    return await Task.FromResult(new AttachmentModel());
                }

                return await Task.FromResult(Attachment);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new AttachmentModel();
            }
        }

        public async Task<string> CreateTest(TestModel request)
        {
            string ValidationResult = Validate(request);
            if (ValidationResult == "")
            {
                try
                {
                    using (ApplicationContext ctx = new ApplicationContext())
                    {
                        var state = await ctx.tests.AddAsync(new TestModel()
                        {
                            uuid = Guid.NewGuid(),
                            parentId = request.parentId,
                            url = request.url,
                        });
                        await ctx.SaveChangesAsync();
                        if (state.State == Microsoft.EntityFrameworkCore.EntityState.Added)
                        {
                            return "Added small module successfully";
                        }
                        return "EntityState.Added is false";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return ex.ToString();
                }
            }
            else
            {
                return ValidationResult;
            }
        }

        public async Task<string> ModifyTest(TestModel request)
        {
            string ValidationResult = Validate(request);
            if (ValidationResult == "")
            {
                try
                {
                    using (ApplicationContext ctx = new ApplicationContext())
                    {
                        var Test = ctx.tests.Where(p => p.uuid == request.uuid).ToList()[0];

                        if (Test != null)
                        {
                            Test.parentId = (request.parentId.ToString() == "") ? request.parentId : Test.parentId;
                            Test.url = request.url ?? Test.url;
                            await ctx.SaveChangesAsync();
                            return "";
                        }

                        return $"Test {request.uuid} does not exist";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return ex.ToString();
                }
            }
            else
            {
                return ValidationResult;
            }
        }

        public async Task<string> DeleteTest(TestModel request)
        {
            try
            {
                using (ApplicationContext ctx = new ApplicationContext())
                {
                    var Test = await ctx.tests.FindAsync(request.uuid);
                    if (Test != null)
                    {
                        ctx.tests.Remove(Test);
                        await ctx.SaveChangesAsync();
                        return "";
                    }

                    return String.Format("User ({0} does not exist", request.uuid);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return ex.ToString();
            }
        }

        public async Task<TestModel> GetTest(TestModel request)
        {
            try
            {
                var Test = new TestModel();
                using (ApplicationContext ctx = new ApplicationContext())
                {

                    Test = await ctx.tests.FindAsync(request.uuid);
                }

                if (Test == null)
                {
                    return await Task.FromResult(new TestModel());
                }

                return await Task.FromResult(Test);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new TestModel();
            }
        }

        public async Task<string> CreateDeadline(DeadlineModel request)
        {
            string ValidationResult = Validate(request);
            if (ValidationResult == "")
            {
                try
                {
                    using (ApplicationContext ctx = new ApplicationContext())
                    {
                        var state = await ctx.deadlines.AddAsync(new DeadlineModel()
                        {
                            uuid = Guid.NewGuid(),
                            parentId = request.parentId,
                            userId = request.userId,
                            deadline = request.deadline

                        });
                        await ctx.SaveChangesAsync();
                        if (state.State == Microsoft.EntityFrameworkCore.EntityState.Added)
                        {
                            return "Added small module successfully";
                        }
                        return "EntityState.Added is false";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return ex.ToString();
                }
            }
            else
            {
                return ValidationResult;
            }
        }

        public async Task<string> ModifyDeadline(DeadlineModel request)
        {
            string ValidationResult = Validate(request);
            if (ValidationResult == "")
            {
                try
                {
                    using (ApplicationContext ctx = new ApplicationContext())
                    {
                        var Deadline = ctx.deadlines.Where(p => p.uuid == request.uuid).ToList()[0];

                        if (Deadline != null)
                        {
                            //check if these actually work or not, I don't have any idea
                            //and I have made no research on it either.
                            Deadline.parentId = (request.parentId.ToString() == "") ? request.parentId : Deadline.parentId;
                            Deadline.userId = (request.userId.ToString() == "") ? request.userId : Deadline.userId;
                            Deadline.deadline = (request.deadline.ToString() == "") ? request.deadline : Deadline.deadline;
                            await ctx.SaveChangesAsync();
                            return "";
                        }

                        return $"Deadline {request.uuid} does not exist";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return ex.ToString();
                }
            }
            else
            {
                return ValidationResult;
            }
        }

        public async Task<string> DeleteDeadline(DeadlineModel request)
        {
            try
            {
                using (ApplicationContext ctx = new ApplicationContext())
                {
                    var Deadline = await ctx.deadlines.FindAsync(request.uuid);
                    if (Deadline != null)
                    {
                        ctx.deadlines.Remove(Deadline);
                        await ctx.SaveChangesAsync();
                        return "";
                    }

                    return String.Format("User ({0} does not exist", request.uuid);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return ex.ToString();
            }
        }

        public async Task<DeadlineModel> GetDeadline(DeadlineModel request)
        {
            try
            {
                var Deadline = new DeadlineModel();
                using (ApplicationContext ctx = new ApplicationContext())
                {

                    Deadline = await ctx.deadlines.FindAsync(request.uuid);
                }

                if (Deadline == null)
                {
                    return await Task.FromResult(new DeadlineModel());
                }

                return await Task.FromResult(Deadline);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new DeadlineModel();
            }
        }

        private string Validate<T>(T t_model)
        {
            try
            {
                if (t_model == null)
                {
                    throw new ArgumentNullException(nameof(t_model));
                }

                var results = new List<ValidationResult>();
                var context = new ValidationContext(t_model);
                string errors = "";
                if (!Validator.TryValidateObject(t_model, context, results, true))
                {
                    foreach (var error in results)
                    {
                        errors += error.ErrorMessage + '\n';
                    }
                }

                return errors;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return ex.ToString();
            }
        }
        
    }
}
