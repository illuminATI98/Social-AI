namespace Social_AI.Services;

public interface IService<T>
{
    public Task Add(T entity);
    
    public Task<T> Get(long id);
    
    public Task<IEnumerable<T>> GetAll();
    
    public Task Update(T entity);
    
    public Task Delete(long id);
}