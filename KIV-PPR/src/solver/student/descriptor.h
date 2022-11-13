/**
 * SmartCGMS - continuous glucose monitoring and controlling framework
 * https://diabetes.zcu.cz/
 *
 * Copyright (c) since 2018 University of West Bohemia.
 *
 * Contact:
 * diabetes@mail.kiv.zcu.cz
 * Medical Informatics, Department of Computer Science and Engineering
 * Faculty of Applied Sciences, University of West Bohemia
 * Univerzitni 8
 * 301 00, Pilsen
 * 
 * 
 * Purpose of this software:
 * This software is intended to demonstrate work of the diabetes.zcu.cz research
 * group to other scientists, to complement our published papers. It is strictly
 * prohibited to use this software for diagnosis or treatment of any medical condition,
 * without obtaining all required approvals from respective regulatory bodies.
 *
 * Especially, a diabetic patient is warned that unauthorized use of this software
 * may result into severe injure, including death.
 *
 *
 * Licensing terms:
 * Unless required by applicable law or agreed to in writing, software
 * distributed under these license terms is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *
 * a) For non-profit, academic research, this software is available under the
 *      GPLv3 license.
 * b) For any other use, especially commercial use, you must contact us and
 *       obtain specific terms and conditions for the use of the software.
 * c) When publishing work with results obtained using this software, you agree to cite the following paper:
 *       Tomas Koutny and Martin Ubl, "Parallel software architecture for the next generation of glucose
 *       monitoring", Procedia Computer Science, Volume 141C, pp. 279-286, 2018
 */

#pragma once

#include "../../common/iface/UIIface.h"
#include "../../common/rtl/hresult.h"


namespace pprsolver {
	constexpr GUID pprsolver_id_serial = { 0xe7e2099f, 0x8f, 0x4f49, { 0x83, 0x8c, 0x63, 0x7c, 0x6b, 0x61, 0x6a, 0xc1 } };
	constexpr wchar_t pprsolver_desc_serial[] = L"David Pivovar - Imperialist Competitive Algorithm - Serial";
	constexpr GUID pprsolver_id_smp = { 0x5097e774, 0x44c9, 0x43e4, { 0xb2, 0x33, 0xf6, 0x8e, 0x77, 0x63, 0x7d, 0xe3 } };
	constexpr wchar_t pprsolver_desc_smp[] = L"David Pivovar - Imperialist Competitive Algorithm - SMP";
	constexpr GUID pprsolver_id_opencl = { 0x308973f6, 0x6eb0, 0x4ebe, { 0x91, 0xd9, 0xa0, 0x92, 0x47, 0xda, 0xb, 0xd0 } };
	constexpr wchar_t pprsolver_desc_opencl[] = L"David Pivovar - Imperialist Competitive Algorithm - OpenCL";
}

extern "C" HRESULT IfaceCalling do_get_solver_descriptors(glucose::TSolver_Descriptor **begin, glucose::TSolver_Descriptor **end);