using Dm.util;
using LT.Framework.Contract.Dtos.Files;
using LT.Framework.Domain.Shared.Ibll;
using LT.Framework.Exam.Application.Contract.Dtos;
using LT.Framework.Exam.Domain.Entities;
using LT.Framework.Exam.Domain.IRepositories;
using LT.Framework.Exam.Domain.Shared.Enum;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Saki.BaseTemplate.BaseControllers;
using Saki.BaseTemplate.Enums;
using Saki.Framework.AppBase;
using Saki.Framework.AppBase.Extensions;

namespace LT.Framework.Exam.Application.Services
{
    [Authorize]
    [Microsoft.AspNetCore.Components.Route("api/[controller]/[action]")]
    public class ExamService : BaseController
    {
        private IExamRepository _examRepository;
        private IExamQuestionRepository _examQuestionRepository;
        private IExamRecordRepository _examRecordRepository;
        private IExamAnswerRepository _examAnswerRepository;
        private IFileService _fileService;
        private ICurrentUser _currentUser;
        public ExamService(IExamRepository examRepository,
            IExamQuestionRepository examQuestionRepository,
            IExamRecordRepository examRecordRepository,
            IExamAnswerRepository examAnswerRepository,
            IFileService fileService,
            ICurrentUser currentUser)
        {
            _examRepository = examRepository;
            _examQuestionRepository = examQuestionRepository;
            _examRecordRepository = examRecordRepository;
            _examAnswerRepository = examAnswerRepository;
            _fileService = fileService;
            _currentUser = currentUser;
        }

        /// <summary>
        /// 添加作业 - 新增一条作业数据
        /// </summary>
        /// <param name="dto">考试主数据实体</param>
        /// <returns></returns>
        public async Task<IActionResult> AddExam(ExamsAddInputDto dto)
        {
            //创建基础对象
            ExamEntity entity = dto.Adapt<ExamEntity>();
            entity.Create(_currentUser.UserId);
            int count = await _examRepository.AsQueryable().CountAsync(
                t => t.Title == dto.Title
                && ((t.StartDate <= dto.StartDate && t.EndDate >= dto.StartDate) || (t.StartDate <= dto.EndDate && t.EndDate >= dto.EndDate)));
            if (count > 0)
            {
                return Fail($"在{dto.StartDate?.ToString("yyyy-MM-dd")}至{dto.EndDate?.ToString("yyyy-MM-dd")}内，已存在{dto.Title}");
            }
            var flage = await _examRepository.Add(entity);
            if (flage)
            {
                return Success(entity.Adapt<ExamsAddOutputDto>());
            }
            else
            {
                return Fail("数据添加失败");
            }
        }

        /// <summary>
        /// 修改作业主表 - 根据Id修改指定作业数据
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdateExam")]
        public async Task<IActionResult> UpdateExam([FromBody]ExamsUpdateInputDto dto)
        {
            //创建基础对象
            ExamEntity entity = dto.Adapt<ExamEntity>();
            entity.Update(_currentUser.UserId);
            var flage = await _examRepository.UpdateNotNull(entity);
            if (flage)
            {
                return Success(entity.Adapt<ExamsAddOutputDto>());
            }
            else
            {
                return Fail("数据更新失败");
            }
        }

        /// <summary>
        /// 删除作业 - 根据Id修改删除作业数据
        /// </summary>
        /// <returns></returns>
        [HttpPost("DeleteExam")]
        public async Task<IActionResult> DeleteExam(string[] ids)
        {
            //创建基础对象
            List<ExamEntity> entities = new List<ExamEntity>();
            foreach (var id in ids)
            {
                entities.Add(new ExamEntity()
                {
                    Id = Guid.Parse(id)
                });
            }
            entities.ForEach(t => t.Delete(_currentUser.UserId));
            var flage = await _examRepository.UpdateNotNull(entities);
            if (flage)
            {
                return Success(entities.Adapt<List<ExamsAddOutputDto>>());
            }
            else
            {
                return Fail("数据删除失败");
            }
        }

        /// <summary>
        /// 获取完整作业信息 - 根据Id获取指定作业信息
        /// 包括作业信息,题目信息,答题信息
        /// </summary>
        /// <param name="examId">作业Id</param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> GetExamInfoById(string examId)
        {
            // 获取考试信息
            var exam = await _examRepository.GetByIdAsync(examId);
            if (exam == null)
            {
                return Fail("未找到指定的考试信息");
            }
            var examInfo = exam.Adapt<ExamsAddOutputDto>();
            if (!string.IsNullOrEmpty(exam.AnswerFileId))
            {
                var file = await _fileService.GetFileInfo(exam.AnswerFileId);
                examInfo.AnswerFileName = file?.FileName ?? null;
            }
            if (!string.IsNullOrEmpty(exam.BGFileId))
            {
                var file = await _fileService.GetFileInfo(exam.BGFileId);
                examInfo.BGFileName = file?.FileName ?? null;
            }
            List<ExamQuestionEntity> questions = await _examQuestionRepository.AsQueryable().Where(t => t.ExamId == exam.Id.ToString()).OrderBy(t => t.QuestionNumber).ToListAsync();
            var questionDict = questions.Adapt<List<ExamQuestionOutputDto>>();
            ExamDetailOutputDto dto = new ExamDetailOutputDto()
            {
                ExamInfo = examInfo,
                Questions = questionDict,
            };
            return Success(dto);
        }

        /// <summary>
        /// 上传作业附件
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> UploadExamFile([FromForm] UploadExamUploadDto dto)
        {
            string description = $"{dto.FileType.GetDescription()}:{dto.File.FileName}";
            if (dto.File == null)
            {
                return Fail("传输文件不能为空");
            }
            var fileInfo = await _fileService.FileSave(new FileUploadRecordInputDto { Description = description }, dto.File);
            ExamEntity exam = new ExamEntity() { Id = Guid.Parse(dto.ExamId) };
            switch (dto.FileType)
            {
                case ExamFileType.AnswersFile:
                    exam.AnswerFileId = fileInfo.Data.Id;
                    break;
                case ExamFileType.BGFile:
                    exam.BGFileId = fileInfo.Data.Id;
                    break;
                default:
                    exam.BGFileId = string.Empty;
                    break;
            }
            bool flag = await _examRepository.UpdateNotNull(exam);
            if (flag)
                return Success(fileInfo);
            else
                return Fail("保存附件数据失败");
        }

        /// <summary>
        /// 添加题目 - 仅添加单个题目的
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> AddQuestion(ExamQuestionAddInputDto dto)
        {
            //创建基础对象
            ExamQuestionEntity entity = dto.Adapt<ExamQuestionEntity>();
            entity.Create();
            var flage = await _examQuestionRepository.Add(entity);
            if (flage)
            {
                return Success(entity.Adapt<ExamQuestionOutputDto>());
            }
            else
            {
                return Fail("数据添加失败");
            }
        }

        /// <summary>
        /// 添加题目 - 批量添加
        /// </summary>
        /// <returns></returns>
        [HttpPost("AddQuestionList")]
        public async Task<IActionResult> AddQuestionList(List<ExamQuestionAddInputDto> dto)
        {
            //创建基础对象
            List<ExamQuestionEntity> examQuestions = dto.Adapt<List<ExamQuestionEntity>>();
            foreach (var item in examQuestions)
            {
                item.Create();
            }
            // 考试Id
            var examId = dto.First().ExamId;
            var flage = await _examQuestionRepository.TranCompletedExamQuestion(examId,examQuestions);
            if (flage)
            {
                return Success(examQuestions.Adapt<List<ExamQuestionOutputDto>>());
            }
            else
            {
                return Fail("数据添加失败");
            }
        }

        /// <summary>
        /// 修改题目 - 修改单个题目数据
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdateQuestion")]
        public async Task<IActionResult> UpdateQuestion(ExamQuestionUpdateInputDto dto)
        {
            //创建基础对象
            ExamQuestionEntity entity = dto.Adapt<ExamQuestionEntity>();
            entity.Update();
            var flage = await _examQuestionRepository.UpdateNotNull(entity);
            if (flage)
            {
                return Success(entity.Adapt<ExamQuestionOutputDto>());
            }
            else
            {
                return Fail("数据添加失败");
            }
        }

        /// <summary>
        /// 删除指定题目 - 根据Id删除题目数据
        /// </summary>
        /// <returns></returns>
        [HttpPost("DeleteQuestion")]
        public async Task<IActionResult> DeleteQuestion(string[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return Fail("未指定要删除的题目");
            }
            var entities = await _examQuestionRepository.GetListAsync(t => ids.Contains(t.Id.ToString()));
            if (entities == null || entities.Count == 0)
            {
                return Fail("未找到指定的题目");
            }
            var flag = await _examQuestionRepository.DeleteByIds(ids);
            if (flag)
            {
                return Success(entities.Adapt<List<ExamQuestionOutputDto>>());
            }
            else
            {
                return Fail("数据删除失败");
            }
        }

        /// <summary>
        /// 创建答题记录 - 学生开始答题
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> AddExamRecord(ExamRecordAddInputDto dto)
        {
            //创建基础对象
            ExamRecordEntity entity = dto.Adapt<ExamRecordEntity>();
            entity.Create(_currentUser.UserId);
            var flag = await _examRecordRepository.Add(entity);
            if (flag)
            {
                return Success(entity.Adapt<ExamRecordAddOutputDto>());
            }
            else
            {
                return Fail("数据添加失败");
            }
        }

        /// <summary>
        /// 更新答题记录 - 仅更新答题状态
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> CommitExamRecord(ExamRecordAddInputDto dto)
        {
            //创建基础对象
            var examRecor = await _examRecordRepository.GetFirstAsync(t => t.StudentId.equals(dto.StudentId) && t.ExamId == dto.ExamId);
            examRecor ??= dto.Adapt<ExamRecordEntity>();
            examRecor.Completed(_currentUser.UserId);
            // 查询答题明细
            var answer = await _examAnswerRepository.GetListAsync(t => t.RecordId == examRecor.Id.ToString());
            examRecor.Score = answer.Sum(t => t.Score ?? 0);
            var flag = await _examRecordRepository.Update(examRecor);
            if (flag)
            {
                return Success(examRecor.Adapt<ExamRecordAddOutputDto>());
            }
            else
            {
                return Fail("数据更新失败");
            }
        }

        /// <summary>
        /// 提交答案 - 提交题目的答案,仅用于提交新的数据
        /// </summary>
        /// <param name="dto">答题信息列表</param>
        /// <returns></returns>
        public async Task<IActionResult> AddExamAnswer(RecordItemAddInputDto dto)
        {
            //创建基础对象
            List<ExamAnswerEntity> examAnswers = dto.AnswerList.Adapt<List<ExamAnswerEntity>>();
            List<Guid> questionId = dto.AnswerList.Select(t => Guid.Parse(t.QuestionId)).ToList();
            // 题目列表
            var questions = await _examQuestionRepository.GetListAsync(t => questionId.Contains(t.Id));
            foreach (var item in examAnswers)
            {
                item.Create(dto.RecordId);
                var question = questions.Find(t => t.Id.equals(item.QuestionId));
                var res = CheckQuestion(question, item);
                item.Score = res?.Score ?? 0;
                item.IsCorrect = res?.IsCorrect ?? false;
            }
            var flag = await _examAnswerRepository.AddRange(examAnswers);
            // 是否正确
            if (flag)
            {
                return Success(
                    new RecordItemAddOutputDto
                    {
                        RecordId = dto.RecordId,
                        ExamAnswers = examAnswers.Adapt<List<ExamAnswerAddOutputDto>>()
                    });
            }
            else
            {
                return Fail("数据添加失败");
            }
        }

        /// <summary>
        /// 提交答案列表 - 提交多个题目的答案
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> CommitExamAnswer(ExamAnswerCommitInputDto dto)
        {
            // 答题记录
            List<ExamAnswerEntity> details = dto.AnswerList.Adapt<List<ExamAnswerEntity>>();
            // 作废旧的答题数据
            var record = await _examRecordRepository.GetFirstAsync(t => t.ExamId.Equals(dto.ExamId) && t.StudentId == dto.StudentId);
            // 存在旧的答题记录时将旧数据作废
            if (record != null)
            {
                record.Cancelled(this._currentUser.UserId);
            }
            // 创建新的答题记录
            ExamRecordEntity recordEntity = dto.Adapt<ExamRecordEntity>();
            recordEntity.Create(_currentUser.UserId);
            recordEntity.Completed(_currentUser.UserId);
            List<Guid> questionId = details.Select(t => Guid.Parse(t.QuestionId)).ToList();
            // 题目列表
            var questions = await _examQuestionRepository.GetListAsync(t => questionId.Contains(t.Id));
            foreach (var item in details)
            {
                item.Create(recordEntity.Id.ToString());
                if (string.IsNullOrEmpty(item.StudentAnswer))
                {
                    item.IsCorrect = null;
                    continue;
                }
                // 获取题目用于判断答案是否正确
                var question = questions.Find(t => t.Id.ToString().ToLower().Equals(item.QuestionId.ToLower()));
                var res = CheckQuestion(question, item);
                // 继承得分
                item.Score = res?.Score ?? 0;
                item.IsCorrect = res?.IsCorrect ?? false;
            }
            recordEntity.Score = details.Sum(t => t.Score ?? 0);
            await _examRecordRepository.TranCompletedExam(recordEntity, details);
            var output = recordEntity.Adapt<ExamRecordAddOutputDto>();
            output.ExamAnswers = details.Adapt<List<ExamAnswerAddOutputDto>>();
            return Success(output);
        }

        /// <summary>
        /// 验证答案是否正确
        /// </summary>
        /// <returns></returns>
        [NonAction]
        private ExamAnswerEntity CheckQuestion(ExamQuestionEntity question, ExamAnswerEntity answer) 
        {
            // 构建一个新的返回参数，不直接修改入参
            var res = answer.Adapt<ExamAnswerEntity>();
            if (string.IsNullOrEmpty(answer.StudentAnswer))
            {
                res.IsCorrect = null;
            }
            else if (question != null && !string.IsNullOrEmpty(question.CorrectAnswer))
            {
                // 判断答案是否正确
                var questionAnswer = question.CorrectAnswer.Split(";").ToList().OrderBy(t => t);
                var studentAnswer = answer.StudentAnswer.Split(";").ToList().OrderBy(t => t);
                if (questionAnswer.SequenceEqual(studentAnswer))
                {
                    res.IsCorrect = true;
                    res.Score = question.Score;
                }
                // 判断答案是否部分正确=>老大说对一半不给你们算分哈哈哈哈哈
                else if (questionAnswer.Intersect(studentAnswer).Any())
                {
                    //res.IsCorrect = null;
                    //res.Score = question.Score / 2.00;
                    res.IsCorrect = false;
                    res.Score = 0;
                }
                // 部分正确暂不处理
                else
                {
                    res.IsCorrect = false;
                    res.Score = 0;
                }
            }
            return res;
        }

        /// <summary>
        /// 上传答题附件
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> UploadExamRecordFile([FromForm]UploadExamRecordFileDto dto)
        {
            string description = $"作业附件:{dto.File.FileName}";
            if (dto.File == null)
            {
                return Fail("传输文件不能为空");
            }
            var fileInfo = await _fileService.FileSave(new FileUploadRecordInputDto { Description = description }, dto.File);
            var examRecore = new ExamRecordEntity() { Id = Guid.Parse(dto.RecordId) };
            examRecore.Update(string.Empty);
            examRecore.FileId = fileInfo.Data.Id;
            bool flag = await _examRecordRepository.UpdateNotNull(examRecore);
            if (flag)
                return Success(examRecore);
            else
                return Fail("保存附件数据失败");
        }

        /// <summary>
        /// 删除答案 - 根据Id删除答题数据
        /// </summary>
        /// <returns></returns>
        [HttpPost("DeleteAnswer")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteAnswer(string[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return Fail("未指定要删除的答案");
            }
            var entities = await _examAnswerRepository.GetListAsync(t => ids.Contains(t.Id.ToString()));
            if (entities == null || entities.Count == 0)
            {
                return Fail("未找到指定的答案");
            }
            var flag = await _examAnswerRepository.DeleteByIds(ids);
            if (flag)
            {
                return Success(entities.Adapt<List<ExamAnswerAddOutputDto>>());
            }
            else
            {
                return Fail("数据删除失败");
            }
        }

        /// <summary>
        /// 分页查询作业列表
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> GetExamPageList(ExamQueryInputDto dto)
        {
            string userId = _currentUser.UserId;
            //创建基础对象
            var pageData = await _examRepository.QueryPageAsync(dto, (query, dto) =>
            {
                query.Where(t => t.IsDeleted == BooleanEnum.NO)
                .WhereIF(string.IsNullOrEmpty(userId), t => t.StartDate <= DateTime.Now && t.EndDate >= DateTime.Now)
                .WhereIF(dto.StartDate != null, t => t.StartDate <= dto.StartDate)
                .WhereIF(dto.EndDate != null, t => t.EndDate >= dto.StartDate)
                .WhereIF(!string.IsNullOrEmpty(dto.Keyword), t => t.Title.Contains(dto.Keyword));
            }, t =>
            {
                t.OrderByDescending(t => t.CreatedTime);
            });
            PageResult<ExamsAddOutputDto> pageResult = new PageResult<ExamsAddOutputDto>()
            {
                PageIndex = pageData.PageIndex,
                PageSize = pageData.PageSize,
                TotalCount = pageData.TotalCount,
                PageList = pageData.PageList.Adapt<List<ExamsAddOutputDto>>(),
            };
            return Success(pageResult);
        }

        /// <summary>
        /// 分页查询作业题目
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> GetExamQuestionPageList(ExamQuestionPageQueryDto dto)
        {
            // 分页查询
            var pageData = await _examQuestionRepository.QueryPageAsync(dto, (query, dto) =>
            {
                query.Where(t => t.ExamId == dto.ExamId);
            }, t =>
            {
                t.OrderBy(t => t.QuestionNumber);
            });
            PageResult<ExamQuestionOutputDto> pageResult = new PageResult<ExamQuestionOutputDto>()
            {
                PageIndex = pageData.PageIndex,
                PageSize = pageData.PageSize,
                TotalCount = pageData.TotalCount,
                PageList = pageData.PageList.Adapt<List<ExamQuestionOutputDto>>(),
            };
            return Success(pageResult);
        }

        /// <summary>
        /// 查询答题详情
        /// </summary>
        /// <param name="recordId">考试记录Id</param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> GetExamDetail(string recordId)
        {
            // 获取答题记录
            var record = await _examRecordRepository.GetByIdAsync(recordId);
            if (record == null)
            {
                return Fail("未找到指定的答题记录");
            }
            // 获取考试信息
            var exam = await _examRepository.GetByIdAsync(record.ExamId);
            if (exam == null)
            {
                return Fail("未找到指定的考试信息");
            }
            List<ExamQuestionEntity> questions = await _examQuestionRepository.AsQueryable().Where(t=>t.ExamId == exam.Id.ToString()).OrderBy(t=>t.QuestionNumber).ToListAsync();
            // 获取答题详情
            List<ExamAnswerEntity> answers = await _examAnswerRepository.AsQueryable().Where(t => t.RecordId ==  recordId.ToString()).ToListAsync();
            var questionDict = questions.Adapt<List<ExamQuestionOutputDto>>();
            foreach (var item in questionDict)
            {
                item.Answer = answers.Find(t=>t.QuestionId.ToLower().equals(item.Id))?.Adapt<ExamAnswerAddOutputDto>()??null;
            }
            ExamDetailOutputDto dto = new ExamDetailOutputDto()
            {
                ExamInfo = exam.Adapt<ExamsAddOutputDto>(),
                Questions = questionDict,
            };
            return Success(dto);
        }
    }
}
