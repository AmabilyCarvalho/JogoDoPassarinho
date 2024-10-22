using System.Threading.Channels;

namespace JogoDoPassarinho;

public partial class MainPage : ContentPage
{

	const int gravidade = 4;
	const int tempoEntreFrames = 25;
	bool estaMorto = false;
	double larguraJanela = 0;
	double alturaJanela = 0;
	int velocidade = 20;
	const int forcaPulo = 30;
	const int maxTempoPulando = 3;
	int tempoPulando = 0;
	bool estaPulando = false;
	int score = 0;
	const int aberturaMinima = 250;



	public MainPage()
	{
		InitializeComponent();
	}

	void GerenciaCanos()
	{
		pilarnormal.TranslationX -= velocidade;
		pilarvirado.TranslationX -= velocidade;
		if (pilarvirado.TranslationX < -larguraJanela)
		{
			pilarvirado.TranslationX = 0;
			pilarnormal.TranslationX = 0;
			var alturaMax = -100;
			var alturaMin = -pilarvirado.HeightRequest;
			pilarnormal.TranslationY = Random.Shared.Next((int)alturaMin, (int)alturaMax);
			pilarvirado.TranslationY = pilarnormal.TranslationY + aberturaMinima + pilarvirado.HeightRequest;
			score++;
			labelScore.Text = "Canos: " + score.ToString("D3");
			ok.Text = "voce morreu mas passou por " + score.ToString("D3") + " canos tente novamente!";
		}
	}

	protected override void OnSizeAllocated(double w, double h)
	{
		base.OnSizeAllocated(w, h);
		larguraJanela = w;
		alturaJanela = h;
	}

	void Inicializar()
	{
		pilarnormal.TranslationX = -larguraJanela;
		pilarvirado.TranslationX = -larguraJanela;
		paimon.TranslationX = 0;
		paimon.TranslationY = 0;
		score = 0;
		GerenciaCanos();
	
	}

	void Oi(object s, TappedEventArgs e)
	{
		FrameGameOver.IsVisible = false;
		estaMorto = false;
		Inicializar();
		Desenha();
	}

	void AplicaGravidade()
	{
		paimon.TranslationY += gravidade;
	}

	void AplicaPulo()
	{
		paimon.TranslationY -= forcaPulo;
		tempoPulando++;
		if (tempoPulando >= maxTempoPulando)
		{
			estaPulando = false;
			tempoPulando = 0;
		}
	}

	public async void Desenha()
	{
		while (!estaMorto)
		{
			if (estaPulando)
				AplicaPulo();
			else
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

	bool VerificaColisao()
   {
		if(!estaMorto)

	
	{
	if(VerificaColisaoTeto() ||
	   VerificaColisaoChao() ||
	   VerificaColisaopilarnormal())
	   {
		return true;
	   }
    }
		return false;
 }
	bool VerificaColisaoTeto()
{
	var minY = -alturaJanela / 2;
	if (paimon.TranslationY <= minY)
		return true;
	else
		return false;
}
bool VerificaColisaoChao()
{
	var maxY = alturaJanela / 2 - graminha.HeightRequest;
	if (paimon.TranslationY >= maxY)
		return true;
	else
		return false;
}

void OnGridClicked(object s, TappedEventArgs a)
{
	estaPulando = true;
}

bool VerificaColisaopilarnormal()
   {
	var posHpaimon = (larguraJanela / 2) - (paimon.WidthRequest / 2);
	var posVpaimon =  (alturaJanela/2) - (paimon.HeightRequest/2) + paimon.TranslationY;
		if (posHpaimon >= Math.Abs(pilarnormal.TranslationX) - pilarnormal.WidthRequest&&
		posHpaimon <= Math.Abs(pilarnormal.TranslationX) + pilarvirado.WidthRequest&&
		posVpaimon <= pilarnormal.HeightRequest + pilarnormal.TranslationY)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}