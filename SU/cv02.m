function cv02()
  fce = input("Function (1 game/2 encrypt/3 decrypt): ")
  
  if(fce == 1)
    game()
  elseif(fce == 2)
    text = input("Text: ",'s')
    key = input("Key: ",'s')
    ciph = encrypt(text, key)
    disp(ciph)
  elseif(fce == 3)
    ciph = input("Cipher: ",'s')
    key = input("Key: ",'s')
    text = decrypt(ciph, key)
    disp(text)
  else
    disp("Function doesn't exists!")
  end
  
end


function game()
  r = randi(10)
  for i=1:7
    disp("Round: ")
    disp(i)
    
    if(round(r) == true)
      disp("Win")
      break
    elseif(i == 7)
      disp("Lose")
    end
    
  end
end

function res = round(r)
  x = input("Enter number 1-10: ")
  res = false
  
  if(x == r)
    disp("=")
    res = true
  elseif(x < r)
    disp("<")
  elseif(x > r)
      disp(">")
  end
end
  
  
function res = encrypt(text, key)
  key = str2num(key)
  if(key > 32)
    disp("Entered key is too large! Max 32")
    return
  end
  
  for(i=1:length(text))
    c(i) = text(i) - key %inverted add/sub against task so cipher can is printable in ascii at larger key constant
  end
  
  for(i=1:length(c))
    tmp(i*2 - 1) = c(i)
    tmp(i*2) = randi(94) + 32 %32-126 ASCII
  end
  
  for(i=1:length(tmp))
    res(i) = char(tmp(i))
  end
end

function res = decrypt(ciph, key)
  key = str2num(key)
  %ciph = str2num(ciph) %decript num version of cipher
  
  for(i=1:length(ciph)/2)
    tmp(i) = ciph(i*2 -1)
  end

  for(i=1:length(tmp))
    c = tmp(i) + key
    res(i) = char(c)
  end
  
end