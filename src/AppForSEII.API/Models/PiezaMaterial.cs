using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppForSEII.API.Models;

[PrimaryKey(nameof(Pieza3DId), nameof(MaterialId))] //cambiado
public class PiezaMaterial
{
    public PiezaMaterial()
    {
        Pieza3D = null!;
        Material = null!;
    }

    public PiezaMaterial(Pieza3D pieza3D, Material material) 
    : this(){
        Pieza3DId = pieza3D.Id;
        MaterialId = material.Id;
        Pieza3D = pieza3D;
        Material = material;
    }

    //[Key, Column(Order = 0)]
    public int Pieza3DId { get; set; }
    public Pieza3D Pieza3D { get; set; }

    //[Key, Column(Order = 1)]
    public int MaterialId { get; set; }
    public Material Material { get; set; }
}