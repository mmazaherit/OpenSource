#swap camera images
import os
from os import rename, listdir
folder='D:/path/outside/'
n_images=200
for i in range(0,n_images):
    f1=folder+'0_'+str(i)+'.jpg'
    f2=folder+'1_'+str(i)+'_.jpg'
    f3=folder+'1_'+str(i)+'.jpg'
    os.rename(f1,f2)    
    os.rename(f3,f1)
    os.rename(f2,f3)
    print 'swap name image '+f1+'with'+f3
    
    
