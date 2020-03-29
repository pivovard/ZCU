## Copyright (C) 2017 Ondřej Pražák
## 
## This program is free software; you can redistribute it and/or modify it
## under the terms of the GNU General Public License as published by
## the Free Software Foundation; either version 3 of the License, or
## (at your option) any later version.
## 
## This program is distributed in the hope that it will be useful,
## but WITHOUT ANY WARRANTY; without even the implied warranty of
## MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
## GNU General Public License for more details.
## 
## You should have received a copy of the GNU General Public License
## along with this program.  If not, see <http://www.gnu.org/licenses/>.

## -*- texinfo -*- 
## @deftypefn {Function File} {@var{retval} =} getLinearRegression (@var{input1}, @var{input2})
##
## @seealso{}
## @end deftypefn

## Author: Ondřej Pražák <ondraprazak@ondraprazak-Inspiron-7548>
## Created: 2017-08-14

function [linReg] = getLinearRegression ()

  linReg.cost  = @linRegCost;
  linReg.opt = @gradientDescent;
  linReg.predict = @linRegPredict;
  linReg.options.alpha = 0.01;
  linReg.options.num_iters = 400;

end
