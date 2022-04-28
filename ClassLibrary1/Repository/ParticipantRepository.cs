using ClassLibrary1.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Repository
{
    public class ParticipantRepository : IParticipantRepository
    {
        private SurveyDbContext context;
        public ParticipantRepository(SurveyDbContext context) => this.context = context;

        //check if participant with specified email and survey id exists. If so return false, otherwise Create Participant and return true
        public async Task<Guid> CreateAsync(Participant participant)
        {
            if (context.Participants == null) throw new ArgumentNullException("Participants not initialised");
            context.Participants.Add(participant);
            int entitiesAdded = await context.SaveChangesAsync();
            return participant.Id;
        }

        public async Task<Participant?> FindParticipant(Guid participantId)
        {
            if (context.Participants == null) throw new ArgumentNullException("Participants not initialised");
            var participant = await context.Participants.FindAsync(participantId);
            return participant;
        }

        public async Task<Participant?> Select(string? participantEmail, Guid surveyId)
        {
            if (context.Participants == null) throw new ArgumentNullException("Participants not initialised");
            var participant = await context.Participants.Include(p => p.Selections).SingleOrDefaultAsync(p => p.SurveyId == surveyId && p.Email == participantEmail);
            return participant;
        }

        /// <summary>
        /// Eager loading
        /// </summary>
        /// <param name="id">survey id</param>
        /// <returns>Survey with associated quesions and options</returns>
        public async Task<Survey?> SelectSurveyForParticipant(Guid participantId)
        {
            Participant? participant = await context!.Participants
                !.Include(p => p.Survey)
                .ThenInclude(s => s!.Questions)
                .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(p => p.Id == participantId);
            return participant!.Survey;
        }

        public async Task<bool> SurveyExists(Guid surveyId)
        {
            return await context!.Surveys!.FindAsync(surveyId) != null;
        }

        public async Task<bool> UpdateAsync(Participant participant)
        {
            Participant? participantFromDb = await context!.Participants!.FindAsync(participant.Id);

            if (participantFromDb == null)
                return false;
            participantFromDb.Selections = participant.Selections;

            int entitiesUpdated = await context.SaveChangesAsync();
            return entitiesUpdated != 0;
        }
    }
}
