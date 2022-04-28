using ClassLibrary1.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ClassLibrary1.DTOs;

namespace ClassLibrary1.Repository
{
    public class SurveyRepository : ISurveyRepository
    {
        private SurveyDbContext? context;
        public SurveyRepository(SurveyDbContext context) => this.context = context;
        public SurveyRepository() { }

        public ICollection<Survey> SelectAll()
        {
            return context.Surveys!.ToList();
        }


        public async Task<ICollection<Survey>> SelectByAdminUserIdAsync(string adminUserId)
        {
            if (context == null) throw new ArgumentNullException("context not initialised");
            if (context.Surveys == null) throw new ArgumentNullException("Surveys not initialized");

            return await context.Surveys.Where(s => s.AdminUserId == adminUserId).ToListAsync();
        }

        public async Task<Guid> CreateAsync(Survey survey)
        {
            if (context == null) throw new ArgumentNullException("context not initialised");
            if (context.Surveys == null) throw new ArgumentNullException("Surveys not initialized");

            context.Surveys.Add(survey);
            await context.SaveChangesAsync();
            return survey.Id;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (context == null) throw new ArgumentNullException("context not initialised");
            if (context.Surveys == null) throw new ArgumentNullException("Surveys not initialized");

            Survey? survey = await context.Surveys.FindAsync(id);
            if (survey == null) { return false; }
            context.Surveys.Remove(survey);
            int rows = context.SaveChanges();
            return rows > 0;
        }

        public async Task<Survey?> SelectByIdAsync(Guid id)
        {
            if (context == null) throw new ArgumentNullException("context not initialised");
            if (context.Surveys == null) throw new ArgumentNullException("Surveys not initialized");

            var survey = await context.Surveys
                .Include(s => s.Questions)
                .ThenInclude(q => q.Options)
                .Include(s => s.Participants)
                .ThenInclude(p => p.Selections)
                .FirstOrDefaultAsync(s => s.Id == id);

            return survey;
        }

        public async Task<ICollection<Participant>> SelectParticipantsBySurveyId(Guid id)
        {
            if (context == null) throw new ArgumentNullException("context not initialised");
            if (context.Participants == null) throw new ArgumentNullException("Surveys not initialized");
            return await context.Participants.Where(p => p.SurveyId == id && p.Selections!.Count > 0).ToListAsync();
        }

        public async Task<Participant?> SelectParticipantById(Guid id)
        {
            if (context == null) throw new ArgumentNullException("context not initialised");
            if (context.Participants == null) throw new ArgumentNullException("Surveys not initialized");

            var participant = await context.Participants
                .Include(p => p.Selections!.OrderBy(s => s.QuestionId))
                .FirstOrDefaultAsync(p => p.Id == id);

            return participant;
        }

        public async Task<List<Question>> SelectQuestionsBySurveyId(Guid id)
        {
            if (context == null) throw new ArgumentNullException("context not initialised");
            if (context.Questions == null) throw new ArgumentNullException("Questions not initialized");

            return await context.Questions.Include(q => q.Options).Where(q => q.SurveyId == id).ToListAsync();
        }
    }
}
