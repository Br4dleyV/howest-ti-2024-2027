# Algoritmes oefening 2

Hieronder vind je de zeven algoritmes voor oefening 2 terug.  `MODULUS` is een algemene constante waarvan de waarde niet belangrijk is. 

Bepaal voor elk algoritme de tijdscomplexiteit.
Doe daarna tijdsmetingen op `TimeComplexity.Cons.dll` en probeer iedere letter te matchen op 1 van deze algoritmes.

### Algoritme 1

```python
def algo1(input_val):
  if input_val <= 1:
    return 1

  mid = input_val // 2

  result = 0
  for i in range(10000000):
    result += i
    result %= MODULUS

  return result + algo1(mid)
```

De tijdscomplexiteit is O(log n) omdat elke keer het resultaat halveert

### Algoritme 2

```python
def algo2(input_val):
  if input_val <= 0:
    return 1

  result = (algo2(input_val - 1) + algo2(input_val - 2) + 1) % MODULUS
  return result
```

De tijdscomplexiteit is O(2^n) omdat het elke keer 2 functies opnieuw uitvoert, dus exponentieel.

### Algoritme 3

```python
def algo3(input_val):
  sum_val = 0
  for i in range(50000000):
    sum_val += input_val
    sum_val %= MODULUS

  return sum_val
```

De tijdscompolexiteit is O(1) want de lus draait altijd 50 miljoen keer.

### Algoritme 4

```python
def algo4(input_val):
  sum_val = 0
  i = input_val
  while i > 1:
    for j in range(i, 500 * input_val):
      sum_val += j
      sum_val %= MODULUS
    i // = 2
  return sum_val
```

De tijdscomplexiteit is O(n log n) omdat de while loop ongeveer log n keer draait, en elke iteratie doet ongeveer O(n) werk

### Algoritme 5

```python
def algo5(input_val):
  sum_val = 0
  for i in range(input_val * 5):
    for j in range(input_val * 8):
      sum_val += input_val
      sum_val %= MODULUS
  return sum_val
```

De tijdscomplexiteit is O(n^2) omdat er twee geneste for loops zijn

### Algoritme 6

```python
def algo6(input_val):
  sum_val = 0
  for i in range(1000 * input_val):
    for j in range(10000):
      sum_val += input_val
      sum_val %= MODULUS
  return sum_val
```

De tijdscomplexiteit is O(n) omdat de binnenste for lus constant is.

### Algoritme 7

```python
def algo7(input_val):
  sum_val = 0
  for i in range(input_val * input_val):
    for j in range(input_val):
      sum_val += input_val
      sum_val % MODULUS
  return sum_val
```

De tijdscomplexiteit is O(n^3) omdat de buitenste for lus n * n doet.


O(1) → Algoritme 3
O(log n) → Algoritme 1
O(n) → Algoritme 6
O(n log n) → Algoritme 4
O(n²) → Algoritme 5
O(n³) → Algoritme 7
O(2^n) → Algoritme 2