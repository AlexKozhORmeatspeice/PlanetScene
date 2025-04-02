public interface IGradientChangable
{
    /*
      Установка параметра видимости планеты,
      где alpha = 0 - закрытая иконка
      alpha = 1 - открытая иконка
    */
    void SetVisibility(float alpha);
    void SetVisibility(bool isVisible);
    string Name { set; }
}
