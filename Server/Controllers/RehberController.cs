using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Rehberv2.Server.Data;
using Rehberv2.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Rehberv2.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RehberController : ControllerBase
    {
        private readonly DataContext _db;

        public RehberController(DataContext db)
        {
            _db = db;
        }

        private readonly string connectionString = "Server=DESKTOP-GBLRM1T\\SQLEXPRESS;database=rehber;Trusted_Connection=true";

        
        [HttpGet("Liste")]
        public async Task<IEnumerable<Rehberv2.Shared.Rehber>> Liste()
        {
            return await Task.Factory.StartNew<IEnumerable<Rehberv2.Shared.Rehber>>(() =>
            {
                return _db.Rehber;
            });
        }

        [HttpGet("Ekle/{isim}&{soyisim}&{cepTel}&{isTel}&{email}")]
        public async Task Ekle(string isim,string soyisim,string cepTel,string isTel,string email)
        {
            SqlConnection baglantiNesnesi = new SqlConnection(connectionString);
            SqlCommand komutNesnesi = new SqlCommand("insert into Rehber(isim,soyisim,cepTel,isTel,email) values(@isim,@soyisim,@cepTel,@isTel,@email)", baglantiNesnesi);
            komutNesnesi.Parameters.AddWithValue("@isim", isim);
            komutNesnesi.Parameters.AddWithValue("@soyisim", soyisim);
            komutNesnesi.Parameters.AddWithValue("@cepTel", cepTel);
            komutNesnesi.Parameters.AddWithValue("@isTel", isTel);
            komutNesnesi.Parameters.AddWithValue("@email", email);

            if (baglantiNesnesi != null && baglantiNesnesi.State == ConnectionState.Closed)
                await baglantiNesnesi.OpenAsync();

            await komutNesnesi.ExecuteNonQueryAsync();

            await baglantiNesnesi.CloseAsync();
            /*
            Rehber eklenecekKisi = new();
            eklenecekKisi.isim = isim;
            eklenecekKisi.soyisim = soyisim;
            eklenecekKisi.cepTel = cepTel;
            eklenecekKisi.isTel = isTel;
            eklenecekKisi.email = email;
            await _db.Rehber.AddAsync(eklenecekKisi);
            await _db.SaveChangesAsync();*/
        }
        
        [HttpGet("Sil/{rehberID}")]
        public async Task Sil(int rehberID)
        {
            SqlConnection baglantiNesnesi = new SqlConnection(connectionString);
            SqlCommand komutNesnesi = new SqlCommand("delete from Rehber where rehberID=@rehberID", baglantiNesnesi);

            komutNesnesi.Parameters.AddWithValue("@rehberID", rehberID);

            if (baglantiNesnesi != null && baglantiNesnesi.State == ConnectionState.Closed)
                await baglantiNesnesi.OpenAsync();

            await komutNesnesi.ExecuteNonQueryAsync();

            await baglantiNesnesi.CloseAsync();

            /*
            var silinecekKisi = await _db.Rehber.FindAsync(rehberID);
            if (silinecekKisi != null)
            {
                _db.Rehber.Remove(silinecekKisi);
                await _db.SaveChangesAsync();
            }
            */
        }


        public async Task<bool> kayitKontrol(Rehberv2.Shared.Rehber kontrolKisi)
        {
            SqlConnection baglantiNesnesi = new SqlConnection(connectionString);
            SqlCommand kontrolKomutNesnesi = new SqlCommand("select * from Rehber where isim=@isim and soyisim=@soyisim", baglantiNesnesi);
            kontrolKomutNesnesi.Parameters.AddWithValue("@isim", kontrolKisi.isim);
            kontrolKomutNesnesi.Parameters.AddWithValue("@soyisim", kontrolKisi.soyisim);

            if (baglantiNesnesi != null && baglantiNesnesi.State == ConnectionState.Closed)
                await baglantiNesnesi.OpenAsync();

            SqlDataReader reader = await kontrolKomutNesnesi.ExecuteReaderAsync();
            if (!reader.HasRows)
                return true;
            else
                return false;
        }

        /*
        [HttpPost]
        public async Task<bool> kayitEkleMethod(Rehberv2.Shared.Rehber eklenecekKisi)
        {
            if (await kayitKontrol(eklenecekKisi) == true)
            {
                SqlConnection baglantiNesnesi = new SqlConnection(connectionString);
                SqlCommand komutNesnesi = new SqlCommand("insert into Rehber(isim,soyisim,cepTel,isTel,email) values(@isim,@soyisim,@cepTel,@isTel,@email)", baglantiNesnesi);
                komutNesnesi.Parameters.AddWithValue("@isim", eklenecekKisi.isim);
                komutNesnesi.Parameters.AddWithValue("@soyisim", eklenecekKisi.soyisim);
                komutNesnesi.Parameters.AddWithValue("@cepTel", eklenecekKisi.cepTel);
                komutNesnesi.Parameters.AddWithValue("@isTel", eklenecekKisi.isTel);
                komutNesnesi.Parameters.AddWithValue("@email", eklenecekKisi.email);

                if (baglantiNesnesi != null && baglantiNesnesi.State == ConnectionState.Closed)
                    await baglantiNesnesi.OpenAsync();

                await komutNesnesi.ExecuteNonQueryAsync();

                await baglantiNesnesi.CloseAsync();

                // EntityFrameWork //
                /*
                await _db.Rehber.AddAsync(eklenecekKisi);
                await _db.SaveChangesAsync();
                *
                return true;
            }
            else
            {
                return false;
            }
        }*/

        [HttpPost("Duzelt")]
        public async Task<bool> Duzelt([FromBody] Rehberv2.Shared.Rehber degisecekKisi)
        {
            var eskiKisi = await _db.Rehber.FindAsync(degisecekKisi.rehberID); // Kişi kendinin ismi ve soyismi kayıtlıyken diğer bilgilerini değiştirmek isteyebilir.
            if (await kayitKontrol(degisecekKisi) == true || (eskiKisi.isim == degisecekKisi.isim && eskiKisi.soyisim == degisecekKisi.soyisim))
            {
                SqlConnection baglantiNesnesi = new SqlConnection(connectionString);
                SqlCommand komutNesnesi = new SqlCommand("update Rehber set isim=@isim,soyisim=@soyisim,cepTel=@cepTel,isTel=@isTel,email=@email where rehberID=@rehberID", baglantiNesnesi);
                komutNesnesi.Parameters.AddWithValue("@isim", degisecekKisi.isim);
                komutNesnesi.Parameters.AddWithValue("@soyisim", degisecekKisi.soyisim);
                komutNesnesi.Parameters.AddWithValue("@cepTel", degisecekKisi.cepTel);
                komutNesnesi.Parameters.AddWithValue("@isTel", degisecekKisi.isTel);
                komutNesnesi.Parameters.AddWithValue("@email", degisecekKisi.email);

                komutNesnesi.Parameters.AddWithValue("@rehberID", degisecekKisi.rehberID);

                if (baglantiNesnesi != null && baglantiNesnesi.State == ConnectionState.Closed)
                    await baglantiNesnesi.OpenAsync();

                await komutNesnesi.ExecuteNonQueryAsync();

                await baglantiNesnesi.CloseAsync();

                //await _db.SaveChangesAsync();
                // EntityFrameWork //
                /*
                var bulunanDegisecekKisi = await _db.Rehber.FindAsync(rehberID);
                if (bulunanDegisecekKisi != null)
                {
                    bulunanDegisecekKisi.isim = degisecekKisi.isim;
                    bulunanDegisecekKisi.soyisim = degisecekKisi.soyisim;
                    bulunanDegisecekKisi.cepTel = degisecekKisi.cepTel;
                    bulunanDegisecekKisi.isTel = degisecekKisi.isTel;
                    bulunanDegisecekKisi.email = degisecekKisi.email;

                    await _db.SaveChangesAsync();
                }
                */
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpPut("{rehberID}")]
        public async Task<bool> kayitGuncelleMethod(int rehberID,[FromBody] Rehberv2.Shared.Rehber degisecekKisi)
        {
            var eskiKisi = await _db.Rehber.FindAsync(rehberID); // Kişi kendinin ismi ve soyismi kayıtlıyken diğer bilgilerini değiştirmek isteyebilir.
            if (await kayitKontrol(degisecekKisi) == true||(eskiKisi.isim==degisecekKisi.isim&&eskiKisi.soyisim==degisecekKisi.soyisim))
            {
                SqlConnection baglantiNesnesi = new SqlConnection(connectionString);
                SqlCommand komutNesnesi = new SqlCommand("update Rehber set isim=@isim,soyisim=@soyisim,cepTel=@cepTel,isTel=@isTel,email=@email where rehberID=@rehberID", baglantiNesnesi);
                komutNesnesi.Parameters.AddWithValue("@isim", degisecekKisi.isim);
                komutNesnesi.Parameters.AddWithValue("@soyisim", degisecekKisi.soyisim);
                komutNesnesi.Parameters.AddWithValue("@cepTel", degisecekKisi.cepTel);
                komutNesnesi.Parameters.AddWithValue("@isTel", degisecekKisi.isTel);
                komutNesnesi.Parameters.AddWithValue("@email", degisecekKisi.email);

                komutNesnesi.Parameters.AddWithValue("@rehberID", rehberID);

                if (baglantiNesnesi != null && baglantiNesnesi.State == ConnectionState.Closed)
                    await baglantiNesnesi.OpenAsync();

                await komutNesnesi.ExecuteNonQueryAsync();

                await baglantiNesnesi.CloseAsync();

                //await _db.SaveChangesAsync();
                // EntityFrameWork //
                /*
                var bulunanDegisecekKisi = await _db.Rehber.FindAsync(rehberID);
                if (bulunanDegisecekKisi != null)
                {
                    bulunanDegisecekKisi.isim = degisecekKisi.isim;
                    bulunanDegisecekKisi.soyisim = degisecekKisi.soyisim;
                    bulunanDegisecekKisi.cepTel = degisecekKisi.cepTel;
                    bulunanDegisecekKisi.isTel = degisecekKisi.isTel;
                    bulunanDegisecekKisi.email = degisecekKisi.email;

                    await _db.SaveChangesAsync();
                }
                */
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpDelete("{rehberID}")]
        public async Task kayitSilMethod(int rehberID)
        {
            SqlConnection baglantiNesnesi = new SqlConnection(connectionString);
            SqlCommand komutNesnesi = new SqlCommand("delete from Rehber where rehberID=@rehberID", baglantiNesnesi);

            komutNesnesi.Parameters.AddWithValue("@rehberID", rehberID);

            if (baglantiNesnesi != null && baglantiNesnesi.State == ConnectionState.Closed)
                await baglantiNesnesi.OpenAsync();

            await komutNesnesi.ExecuteNonQueryAsync();

            await baglantiNesnesi.CloseAsync();

            // EntityFrameWork //
            /*
            var silinecekKisi = await _db.Rehber.FindAsync(rehberID);
            if (silinecekKisi != null)
            {
                _db.Rehber.Remove(silinecekKisi);
                await _db.SaveChangesAsync();
            }
            */
        }
    }
}
