using System.Threading.Channels;

namespace JogoDoPassarinho;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();
	}

	bool VerificaColisao()
	{
		if(!estaMorto)
	
	{
	if(VerificaColisaoTeto() ||
	   VerificaColisaoChao())
	   {
		return true;
	   }
    }
		return false;
 }
	bool VerificaColisaoTeto()
	{
		var minY = -alturaJanela/2;
		if (paimon.TranslationY <= minY)
		return true;
	else
		return false;
	}
	bool VerificaColisaoChao()
	{
		var maxY = alturaJanela/2 - graminha.HeightRequest;
		if (paimon.TranslationY >= maxY)
		return true;
	else
		return false;
	}
	async Task Desenha()
	{
		while (!estaMorto)
		{
			AplicaGravidade();
			GerenciaCanos();
			if (VerificaColisao())
			{
				estaMorto = true;
				FrameGameOver.IsVisible = true;
				break;
			}
			await Task.Delay(tempoEntreFrames);
		}
	}
}