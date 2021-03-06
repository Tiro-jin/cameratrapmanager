﻿/*
 * Creado por SharpDevelop.
 * Usuario: BENITO
 * Fecha: 12/06/2011
 * Hora: 21:16
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Collections;
using System.Collections.Generic;

using CameratrapManager_lib.CameratrapModel;
using CameratrapManager_lib.CameratrapModel.SampleObservations;
using CameratrapManager_lib.Analysis.Queries;


namespace CameratrapManager_lib.Analysis
{
	/// <summary>
	/// Description of Project_queries.
	/// </summary>
	public class General_queries
	{
		public General_queries(Project currentProject)
		{
			GetAllStatsBySpecies(currentProject);
			GetAllStatsByStation(currentProject);
		}
		
		
		List<SpeciesStats> _allStatsBySpecies=new List<SpeciesStats>();
		List<StationStats> _allStatsByStation=new List<StationStats>();
		
		
		public List<SpeciesStats> AllStatsBySpecies {
			get { return _allStatsBySpecies; }
		}
		
		
		public List<StationStats> AllStatsByStation {
			get { return _allStatsByStation; }
		}
		
		
		private void GetAllStatsByStation(Project currentProject)
		{
			try {
				foreach(Station st in currentProject.StationsList)
				{
					StationStats stst=new StationStats(st.Guid,st.StationID);
					
					foreach(string s in SpeciesInProject(currentProject))//SpeciesInStation(st))
					{
						stst.SpeciesStats.Add(StatsByStation(s,st));
					}
					
					_allStatsByStation.Add(stst);
				}
			} catch (Exception ex) {
				throw ex;
			}

			
		}

		
		private void GetAllStatsBySpecies(Project currentProject)
		{		
			try {
				foreach(string s in SpeciesInProject(currentProject))
				{
					_allStatsBySpecies.Add(StatsBySpecies(s,currentProject));
				}
			} catch (Exception ex) {
				throw ex;
			}
			
		}
		
		
		private SpeciesStats StatsByStation(string species, Station st)
		{
			try {
				int speciesCount=0;
				int speciesPictures=0;
				
				foreach(SpeciesObservation sp in SpeciesObservationsInStation(st))
				{
					if(sp.Value==species)
					{
						speciesCount+=sp.Count;
						speciesPictures++;
					}
				}

				SpeciesStats spst=new SpeciesStats(species,speciesCount,speciesPictures);
				
				return spst;
			} catch (Exception ex) {
				throw ex;
			}

		}
		
		
		private SpeciesStats StatsBySpecies(string species, Project currentProject)
		{
			try {
				int speciesCount=0;
				int speciesPictures=0;
				
				double[] activityPaterns = new double[24];
				
				
				foreach (Station st in currentProject.StationsList)
				{
					foreach(Sample smp in st.SamplesList)
					{
						foreach(SpeciesObservation obs in smp.Species_Observations_list)
						{
							if(obs.Value==species)
							{
								speciesCount+=obs.Count;
								speciesPictures++;
								
								activityPaterns[smp.DateTime.Hour]+=smp.DateTime.Hour;
							}
						}
					}
				}
				

				SpeciesStats spst=new SpeciesStats(species,speciesCount,speciesPictures);
				spst.ActivityPatern=activityPaterns;
				
				
				return spst;
				
			} catch (Exception ex) {
				throw ex;
			}

		}
		
		
		private List<string> SpeciesInStation(Station currentStation)
		{
			try {
				List<string> _allSpeciesList = new List<string>();
				

				foreach(Sample smp in currentStation.SamplesList)
				{
					foreach(Observation obs in smp.Species_Observations_list)
					{
						if(_allSpeciesList.Contains((string)obs.Value)== false)
						{
							_allSpeciesList.Add((string)obs.Value);
						}
					}
				}
				
				return _allSpeciesList;
			} catch (Exception ex) {
				throw ex;
			}

		}
		
		
		private List<string> SpeciesInProject(Project currentProject)
		{
			try {
				List<string> _allSpeciesList = new List<string>();
				
				foreach (Station st in currentProject.StationsList)
				{
					foreach(Sample smp in st.SamplesList)
					{
						foreach(SpeciesObservation obs in smp.Species_Observations_list)
						{
							if(_allSpeciesList.Contains((string)obs.Value)== false)
							{
								_allSpeciesList.Add((string)obs.Value);
							}

						}
					}
				}
				
				return _allSpeciesList;
			} catch (Exception ex) {
				throw ex;
			}
			

		}
		
		
		private List<SpeciesObservation> SpeciesObservationsInStation(Station currentStation)
		{
			try {
				List<SpeciesObservation> _allSpeciesObservations=new List<SpeciesObservation>();
				
				foreach(Sample smp in currentStation.SamplesList)
				{
					foreach(SpeciesObservation obs in smp.Species_Observations_list)
					{
						_allSpeciesObservations.Add(obs);
					}
				}
				
				return _allSpeciesObservations;
			} catch (Exception ex) {
				throw ex;
			}
			
			
		}		
		
		
		
		
	}
}
