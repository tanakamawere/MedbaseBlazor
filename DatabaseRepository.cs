using MedbaseLibrary.Models;
using MedbaseLibrary.Services;

namespace MedbaseBlazor
{
    public class DatabaseRepository : IDatabaseRepository
    {
        public Task DeleteTopicAsync(int topic)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Course> GetCoursesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<QuestionPagedWithTopic> GetPagedQuestionsWithTopic(int topic, int page, double numResults)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Question> GetQuestionsAsync(int topicReference)
        {
            throw new NotImplementedException();
        }

        public Task<QuestionPaged> GetQuestionsPaged(int topic, int page, double numResults)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Question> GetQuizQuestionsAsync(int topicReference, int number)
        {
            throw new NotImplementedException();
        }

        public Task<QuestionPaged> GetSearchQuestionsPaged(int topic, int page, double numResults, string keyword)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Topic> GetTopicsAsync()
        {
            throw new NotImplementedException();
        }

        public Task SaveTopicAndQuestionsAsync(IEnumerable<Question> questions, Topic topic)
        {
            throw new NotImplementedException();
        }
    }
}
