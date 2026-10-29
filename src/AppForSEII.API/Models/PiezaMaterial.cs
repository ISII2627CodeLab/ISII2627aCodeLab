using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppForSEII.API.Models;

public class PiezaMaterial
{
    public PiezaMaterial()
    {
        Pieza3D = null!;
        Material = null!;
    }

    public PiezaMaterial(int pieza3DId, int materialId) 
    : this(){
        Pieza3DId = pieza3DId;
        MaterialId = materialId;
    }

    [Key, Column(Order = 0)]
    public int Pieza3DId { get; set; }
    public Pieza3D Pieza3D { get; set; }

    [Key, Column(Order = 1)]
    public int MaterialId { get; set; }
    public Material Material { get; set; }
}