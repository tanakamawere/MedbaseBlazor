using MedbaseBlazor.Models;

namespace MedbaseBlazor.Repositories
{
    public class ApiRepository : IApiRepository 
    {
        private readonly HttpClient httpClient;
        public ApiRepository(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }
        public async Task<IEnumerable<Question>> GetQuizQuestions(int topic, int number)
        {
            return await httpClient.GetFromJsonAsync<IEnumerable<Question>>($"questions/quiz/{topic}/{number}");
        }

        public async Task<IEnumerable<Article>> GetArticlesNumbered(int num)
        {
            return await httpClient.GetFromJsonAsync<IEnumerable<Article>>($"articles/select/{num}");
        }

        public async Task<IEnumerable<Article>> GetArticles()
        {
            return await httpClient.GetFromJsonAsync<IEnumerable<Article>>("articles");
        }

        public async Task<Article> GetArticle(int id)
        {
            return await httpClient.GetFromJsonAsync<Article>($"/articles/{id}");
        }

        public async Task<IEnumerable<Topic>> GetTopics(string id)
        {
            return await httpClient.GetFromJsonAsync<IEnumerable<Topic>>($"topics/{id}");
        }

        public async Task<IEnumerable<Course>> GetCourses()
        {
            return await httpClient.GetFromJsonAsync<IEnumerable<Course>>($"courses");
        }

        public async Task<IEnumerable<Question>> GetQuestionsSimple(long id)
        {
            return await httpClient.GetFromJsonAsync<IEnumerable<Question>>($"questions/{id}");
        }

        public async Task<QuestionPaged> GetPagedQuestions(int topic, int page, double numResults = 10f)
        {
            return await httpClient.GetFromJsonAsync<QuestionPaged>($"questions/{topic}/{numResults}/{page}");
        }

        public async Task<QuestionPaged> GetSearchPagedQuestions(int topic, int page, double numResults, string keyword)
        {
            return await httpClient.GetFromJsonAsync<QuestionPaged>($"questions/{topic}/{numResults}/{page}/{keyword}");
        }

        public async Task<IEnumerable<Topic>> GetAllTopics()
        {
            return await httpClient.GetFromJsonAsync<IEnumerable<Topic>>("topics");
        }

        public async Task<Question> GetQuestion(int id)
        {
            return await httpClient.GetFromJsonAsync<Question>($"questions/select/{id}");
        }

        public async Task<Topic> GetTopic(int id)
        {
            return await httpClient.GetFromJsonAsync<Topic>($"topics/select/{id}");
        }

        public async Task<IEnumerable<Subscription>> GetSubscriptions()
        {
            return await httpClient.GetFromJsonAsync<IEnumerable<Subscription>>($"subscriptions");
        }

        public async Task<Subscription> GetSubscription(string email)
        {
            return await httpClient.GetFromJsonAsync<Subscription>($"subscriptions/{email}");
        }

        public async Task<Course> GetCourse(int id)
        {
            return await httpClient.GetFromJsonAsync<Course>($"courses/{id}");
        }
        public async Task<IEnumerable<Question>> GetAllQuestions()
        {
            return await httpClient.GetFromJsonAsync<IEnumerable<Question>>($"questions");
        }

        public async Task<bool> PostArticle(Article article)
        {
            var response = await httpClient.PostAsJsonAsync($"articles/{article}", article);
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }

        public async Task<bool> PostTopic(Topic topic)
        {
            var response = await httpClient.PostAsJsonAsync($"topics/{topic}", topic);
               if(response.IsSuccessStatusCode)
                    return true;
                else
                return false;
        }

        public async Task<bool> PostCourse(Course course)
        {
            var response = await httpClient.PostAsJsonAsync($"courses/{course}", course);
            if(response.IsSuccessStatusCode)
                    return true;
            else
                return false;
        }
        public async Task<bool> PostQuestion(Question question)
        {
            var response = await httpClient.PostAsJsonAsync($"questions/{question}", question);
            if(response.IsSuccessStatusCode)
                    return true;
            else
                return false;
        }
        public async Task<bool> PostSubscription(Subscription subscription)
        {
            var response = await httpClient.PostAsJsonAsync($"subscriptions/{subscription}", subscription);
            if(response.IsSuccessStatusCode)
                    return true;
            else
                return false;
        }

        public async void DeleteCourse(int id)
        {
            await httpClient.DeleteAsync($"courses/{id}");
        }

        public async void DeleteQuestion(int id)
        {
            await httpClient.DeleteAsync($"questions/{id}");
        }

        public async void DeleteTopic(int id)
        {
            await httpClient.DeleteAsync($"topics/{id}");
        }

        public async void DeleteArticle(int id)
        {
            await httpClient.DeleteAsync($"articles/{id}");
        }

        public async void DeleteSubscription(int id)
        {
            await httpClient.DeleteAsync($"subscriptions/{id}");
        }

        public async void UpdateQuestion(int id, Question question)
        {
            await httpClient.PutAsJsonAsync($"questions/{id}", question);
        }

        public async void UpdateTopic(int id, Topic topic)
        {
            await httpClient.PutAsJsonAsync($"topics/{id}",topic);
        }

        public async void UpdateCourse(int id, Course course)
        {
            await httpClient.PutAsJsonAsync($"courses/{id}", course);
        }

        public async void UpdateArticle(int id, Article article)
        {
            await httpClient.PutAsJsonAsync($"articles/{id}",article);
        }
        public async void UpdateSubscription(int id, Subscription subscription)
        {
            await httpClient.PutAsJsonAsync($"subscriptions/{id}", subscription);
        }

        public async Task<IEnumerable<Corrections>> GetCorrections()
        {
            return await httpClient.GetFromJsonAsync<IEnumerable<Corrections>>($"corrections");
        }

        public async Task<bool> PostCorrection(Corrections corrections)
        {
            var response = await httpClient.PostAsJsonAsync($"corrections/{corrections}", corrections);
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }

        public async Task DeleteCorrection(int id)
        {
            await httpClient.DeleteAsync($"corrections/deleteone/{id}");
        }

        public async Task MergeCorrections()
        {
            await httpClient.PostAsync("corrections/merge", null);
        }

        public async Task ClearAllCorrection()
        {
            await httpClient.DeleteAsync("corrections/clearall");
        }
    }
}
