namespace Estudos.Model;
public class Produto
{

    public int Id { get; set; }
    public string? Nome  {get; set; }
    public decimal? Preco { get; set; }

    public int? EmpresaId { get; set;}
    //Relacionamento um pra muitos /  No caso um produto só pode ter uma empresa e uma empresa pode ter varios produtos
    public Empresa? Empresa { get; set;}

}