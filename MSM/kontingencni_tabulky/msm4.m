close all
clear all
clc

pkg load statistics


data=cat(3,[120,20;30,30],[50,20;10,20])
return
M1=nan(size(data))
M2_1=nan(size(data))
M2_2=nan(size(data))
M2_3=nan(size(data))
M3_1=nan(size(data))
M3_2=nan(size(data))
M3_3=nan(size(data))

n=sum(sum(sum(data,3),2),1)

for i=1:size(data,1)
  for j=1:size(data,2)
    for k=1:size(data,3)
      M1(i,j,k)=sum(sum(data(i,:,:),3),2)*sum(sum(data(:,j,:),3),1)*sum(sum(data(:,:,k),2),1)/n^2
            
      M2_1(i,j,k)=sum(data(i,j,:),3)*sum(sum(data(:,:,k),2),1)/n
      M2_2(i,j,k)=sum(data(i,:,k),2)*sum(sum(data(:,j,:),3),1)/n
      M2_3(i,j,k)=sum(data(:,j,k),1)*sum(sum(data(i,:,:),3),2)/n
            
      M3_1(i,j,k)=sum(data(i,:,k),2)*sum(data(:,j,k),1)/sum(sum(data(:,:,k),2),1)
      M3_2(i,j,k)=sum(data(i,j,:),3)*sum(data(:,j,k),1)/sum(sum(data(:,j,:),3),1)
      M3_3(i,j,k)=sum(data(i,j,:),3)*sum(data(i,:,k),2)/sum(sum(data(i,:,:),3),2)
    end
  end
end

M4=ones(size(data))
tmp = data
r=0

while(sum(sum(sum((M4-tmp).^2,3),2),1) > 0.01)
  r=r+1
  %3r
  tmp=M4  
  for k=1:1:size(data,3)
    for j=1:1:size(data,2)
      for i=1:1:size(data,1)
        M4(i,j,k)=tmp(i,j,k)*sum(data(i,j,:),3)/sum(tmp(i,j,:),3)
      end
    end
  end
  %3r+1
  tmp=M4 
  for k=1:1:size(data,3)
    for j=1:1:size(data,2)
      for i=1:1:size(data,1)
        M4(i,j,k)=tmp(i,j,k)*sum(data(i,:,k),2)/sum(tmp(i,:,k),2)
      end
    end
  end
  %3r+2
  tmp=M4 
  for k=1:1:size(data,3)
    for j=1:1:size(data,2)
      for i=1:1:size(data,1)
        M4(i,j,k)=tmp(i,j,k)*sum(data(:,j,k),1)/sum(tmp(:,j,k),1)
      end
    end
  end
end


krit(1,1)=sum(sum(sum((data-M1).^2./M1,3)))        %T
krit(1,2)=2*sum(sum(sum(data.*log(data./M1),3)))   %G
st_vol(1)=size(data,1)*size(data,2)*size(data,3)-(size(data,1)+size(data,2)+size(data,3))+2

krit(2,1)=sum(sum(sum((data-M2_1).^2./M2_1,3)))      %T
krit(2,2)=2*sum(sum(sum(data.*log(data./M2_1),3)))   %G
st_vol(2)=(size(data,1)*size(data,2)-1)*(size(data,3)-1)

krit(3,1)=sum(sum(sum((data-M2_2).^2./M2_2,3)))      %T
krit(3,2)=2*sum(sum(sum(data.*log(data./M2_2),3)))   %G
st_vol(3)=(size(data,1)*size(data,3)-1)*(size(data,2)-1)

krit(4,1)=sum(sum(sum((data-M2_3).^2./M2_3,3)))      %T
krit(4,2)=2*sum(sum(sum(data.*log(data./M2_3),3)))   %G
st_vol(4)=(size(data,3)*size(data,2)-1)*(size(data,1)-1)


krit(5,1)=sum(sum(sum((data-M3_1).^2./M3_1,3)))      %T
krit(5,2)=2*sum(sum(sum(data.*log(data./M3_1),3)))   %G
st_vol(5)=(size(data,1)-1)*(size(data,2)-1)*size(data,3)

krit(6,1)=sum(sum(sum((data-M3_2).^2./M3_2,3)))      %T
krit(6,2)=2*sum(sum(sum(data.*log(data./M3_2),3)))   %G
st_vol(6)=(size(data,1)-1)*(size(data,3)-1)*size(data,2)

krit(7,1)=sum(sum(sum((data-M3_3).^2./M3_3,3)))      %T
krit(7,2)=2*sum(sum(sum(data.*log(data./M2_3),3)))   %G
st_vol(7)=(size(data,3)-1)*(size(data,2)-1)*size(data,1)

krit(8,1)=sum(sum(sum((data-M4).^2./M4,3)))        %T
krit(8,2)=2*sum(sum(sum(data.*log(data./M4),3)))   %G
st_vol(8)=(size(data,1)-1)*(size(data,2)-1)*(size(data,3)-1)


P(:,1)=1-chi2cdf(krit(:,1),st_vol')
P(:,2)=1-chi2cdf(krit(:,2),st_vol')


